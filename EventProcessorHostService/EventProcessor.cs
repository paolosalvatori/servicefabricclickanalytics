#region Copyright

// //=======================================================================================
// // Microsoft Azure Customer Advisory Team  
// //
// // This sample is supplemental to the technical guidance published on the community
// // blog at http://blogs.msdn.com/b/paolos/. 
// // 
// // Author: Paolo Salvatori
// //=======================================================================================
// // Copyright © 2016 Microsoft Corporation. All rights reserved.
// // 
// // THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// // EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// // MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
// //=======================================================================================

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AzureCat.Samples.Entities;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

#endregion

namespace Microsoft.AzureCat.Samples.EventProcessorHostService
{
    public class EventProcessor : IEventProcessor
    {
        #region Private Static Fields

        private static readonly Dictionary<string, CloudAppendBlob> cloudAppendBlobDictionary =
            new Dictionary<string, CloudAppendBlob>();

        #endregion

        #region Public Constructors

        public EventProcessor(
            string serviceBusConnectionString,
            string storageAccountConnectionString,
            string containerName,
            string queueName,
            int checkpointCount)
        {
            if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
                throw new ArgumentNullException($"{nameof(serviceBusConnectionString)} parameter cannot be null");
            if (string.IsNullOrWhiteSpace(storageAccountConnectionString))
                throw new ArgumentNullException($"{nameof(storageAccountConnectionString)} parameter cannot be null");
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentNullException($"{nameof(containerName)} parameter cannot be null");
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentNullException($"{nameof(queueName)} parameter cannot be null");
            this.checkpointCount = checkpointCount;

            // Creates CloudStorageAccount instance
            CloudStorageAccount cloudStorageAccount;
            CloudStorageAccount.TryParse(storageAccountConnectionString, out cloudStorageAccount);

            //Creates service client for credentialed access to the Blob service.
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            //Gets a reference to a container.
            container = blobClient.GetContainerReference(containerName);

            // Creates a QueueClient
            queueClient = QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);

            // Creates the queue if doesn't exist
            var namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            if (!namespaceManager.QueueExists(queueName))
                namespaceManager.CreateQueue(
                    new QueueDescription(queueName)

                    {
                        EnablePartitioning = true
                    });
        }

        #endregion

        #region Private Fields

        private readonly CloudBlobContainer container;
        private readonly QueueClient queueClient;
        private readonly int checkpointCount;
        private int messageCount;

        #endregion

        #region IEventProcessor Methods

        public Task OpenAsync(PartitionContext context)
        {
            ServiceEventSource.Current.Message(
                $"Lease acquired: EventHub=[{context.EventHubPath}] ConsumerGroup=[{context.ConsumerGroupName}] PartitionId=[{context.Lease.PartitionId}]");
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> events)
        {
            try
            {
                if (events == null)
                    return;
                var eventDataList = events as IList<EventData> ?? events.ToList();

                // Trace individual events
                foreach (var eventData in eventDataList)
                    try
                    {
                        if (!eventData.Properties.ContainsKey("userId"))
                            continue;
                        if (!eventData.Properties.ContainsKey("eventType"))
                            continue;
                        var userId = eventData.Properties["userId"] as string;
                        if (string.IsNullOrWhiteSpace(userId))
                            continue;

                        EventType eventType;
                        var type = eventData.Properties["eventType"];
                        if (type is EventType)
                        {
                            // Casting type
                            eventType = (EventType) type;
                        }
                        else if (type is int)
                        {
                            // Unboxing type
                            eventType = (EventType) (int) type;
                        }
                        else
                        {
                            var value = type as string;
                            if (value != null)
                            {
                                if (!Enum.TryParse(value, out eventType))
                                    continue;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        var cloudAppendBlob = eventType == EventType.StartSession
                            ? NewCloudAppendBlob(userId)
                            : GetCloudAppendBlob(userId);

                        if (cloudAppendBlob == null)
                            continue;
                        using (var stream = new MemoryStream(eventData.GetBytes()))
                        {
                            await cloudAppendBlob.AppendBlockAsync(stream);
                            ServiceEventSource.Current.Message($"Event appended to [{cloudAppendBlob.Name}] ");
                        }

                        // Sends a message to a Service Bus queue containing the address of the append blob
                        // containing the user session events, any time the user session is complete
                        if (eventType == EventType.StopSession)
                        {
                            ServiceEventSource.Current.Message($"User session closed: UserId=[{userId}]");
                            await SendMessageAsync(userId, cloudAppendBlob.Uri);
                        }

                        // Increase messageCount
                        messageCount++;

                        // Invoke CheckpointAsync when messageCount => checkpointCount
                        if (messageCount < checkpointCount)
                            continue;
                        await context.CheckpointAsync();
                        messageCount = 0;
                    }
                    catch (LeaseLostException ex)
                    {
                        // Trace Exception as message
                        ServiceEventSource.Current.Message(ex.Message);
                    }
                    catch (AggregateException ex)
                    {
                        // Trace Exception
                        foreach (var exception in ex.InnerExceptions)
                            ServiceEventSource.Current.Message(exception.Message);
                    }
                    catch (Exception ex)
                    {
                        // Trace Exception
                        ServiceEventSource.Current.Message(ex.Message);
                    }
            }
            catch (LeaseLostException ex)
            {
                // Trace Exception as message
                ServiceEventSource.Current.Message(ex.Message);
            }
            catch (AggregateException ex)
            {
                // Trace Exception
                foreach (var exception in ex.InnerExceptions)
                    ServiceEventSource.Current.Message(exception.Message);
            }
            catch (Exception ex)
            {
                // Trace Exception
                ServiceEventSource.Current.Message(ex.Message);
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            try
            {
                ServiceEventSource.Current.Message(
                    $"Lease lost: EventHub=[{context.EventHubPath}] ConsumerGroup=[{context.ConsumerGroupName}] PartitionId=[{context.Lease.PartitionId}]");
                if (reason == CloseReason.Shutdown)
                    await context.CheckpointAsync();
            }
            catch (Exception ex)
            {
                // Trace Exception
                ServiceEventSource.Current.Message(ex.Message);
            }
        }

        #endregion

        #region Private Static Methods

        private CloudAppendBlob GetCloudAppendBlob(string userId)
        {
            lock (cloudAppendBlobDictionary)
            {
                if (cloudAppendBlobDictionary.ContainsKey(userId))
                    return cloudAppendBlobDictionary[userId];
                return NewCloudAppendBlob(userId);
            }
        }

        private CloudAppendBlob NewCloudAppendBlob(string userId)
        {
            lock (cloudAppendBlobDictionary)
            {
                var cloudAppendBlob =
                    container.GetAppendBlobReference(
                        $"{userId}_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture)}.log");
                if (!cloudAppendBlob.Exists())
                    cloudAppendBlob.CreateOrReplace();
                cloudAppendBlobDictionary[userId] = cloudAppendBlob;
                return cloudAppendBlobDictionary[userId];
            }
        }

        private async Task SendMessageAsync(string userId, Uri blobUri)
        {
            using (var message = new BrokeredMessage(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        new UserSession
                        {
                            UserId = userId,
                            Uri = blobUri
                        }))))
            {
                await queueClient.SendAsync(message);
            }
        }

        #endregion
    }
}