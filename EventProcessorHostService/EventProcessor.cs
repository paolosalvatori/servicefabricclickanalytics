// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.EventProcessorHostService
{
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

    public class EventProcessor : IEventProcessor
    {
        #region Private Static Fields

        private static readonly Dictionary<string, CloudAppendBlob> cloudAppendBlobDictionary = new Dictionary<string, CloudAppendBlob>();

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
            {
                throw new ArgumentNullException($"{nameof(serviceBusConnectionString)} parameter cannot be null");
            }
            if (string.IsNullOrWhiteSpace(storageAccountConnectionString))
            {
                throw new ArgumentNullException($"{nameof(storageAccountConnectionString)} parameter cannot be null");
            }
            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException($"{nameof(containerName)} parameter cannot be null");
            }
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException($"{nameof(queueName)} parameter cannot be null");
            }
            this.checkpointCount = checkpointCount;

            // Creates CloudStorageAccount instance
            CloudStorageAccount cloudStorageAccount;
            CloudStorageAccount.TryParse(storageAccountConnectionString, out cloudStorageAccount);

            //Creates service client for credentialed access to the Blob service.
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

            //Gets a reference to a container.
            this.container = blobClient.GetContainerReference(containerName);

            // Creates a QueueClient
            this.queueClient = QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);

            // Creates the queue if doesn't exist
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(
                    new QueueDescription(queueName)

                    {
                        EnablePartitioning = true
                    });
            }
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
                {
                    return;
                }
                IList<EventData> eventDataList = events as IList<EventData> ?? events.ToList();

                // Trace individual events
                foreach (EventData eventData in eventDataList)
                {
                    try
                    {
                        if (!eventData.Properties.ContainsKey("userId"))
                        {
                            continue;
                        }
                        if (!eventData.Properties.ContainsKey("eventType"))
                        {
                            continue;
                        }
                        string userId = eventData.Properties["userId"] as string;
                        if (string.IsNullOrWhiteSpace(userId))
                        {
                            continue;
                        }

                        EventType eventType;
                        object type = eventData.Properties["eventType"];
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
                            string value = type as string;
                            if (value != null)
                            {
                                if (!Enum.TryParse(value, out eventType))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }

                        CloudAppendBlob cloudAppendBlob = eventType == EventType.StartSession
                            ? this.NewCloudAppendBlob(userId)
                            : this.GetCloudAppendBlob(userId);

                        if (cloudAppendBlob == null)
                        {
                            continue;
                        }
                        using (MemoryStream stream = new MemoryStream(eventData.GetBytes()))
                        {
                            await cloudAppendBlob.AppendBlockAsync(stream);
                            ServiceEventSource.Current.Message($"Event appended to [{cloudAppendBlob.Name}] ");
                        }

                        // Sends a message to a Service Bus queue containing the address of the append blob
                        // containing the user session events, any time the user session is complete
                        if (eventType == EventType.StopSession)
                        {
                            ServiceEventSource.Current.Message($"User session closed: UserId=[{userId}]");
                            await this.SendMessageAsync(userId, cloudAppendBlob.Uri);
                        }

                        // Increase messageCount
                        this.messageCount++;

                        // Invoke CheckpointAsync when messageCount => checkpointCount
                        if (this.messageCount < this.checkpointCount)
                        {
                            continue;
                        }
                        await context.CheckpointAsync();
                        this.messageCount = 0;
                    }
                    catch (LeaseLostException ex)
                    {
                        // Trace Exception as message
                        ServiceEventSource.Current.Message(ex.Message);
                    }
                    catch (AggregateException ex)
                    {
                        // Trace Exception
                        foreach (Exception exception in ex.InnerExceptions)
                        {
                            ServiceEventSource.Current.Message(exception.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Trace Exception
                        ServiceEventSource.Current.Message(ex.Message);
                    }
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
                foreach (Exception exception in ex.InnerExceptions)
                {
                    ServiceEventSource.Current.Message(exception.Message);
                }
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
                {
                    await context.CheckpointAsync();
                }
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
                {
                    return cloudAppendBlobDictionary[userId];
                }
                return this.NewCloudAppendBlob(userId);
            }
        }

        private CloudAppendBlob NewCloudAppendBlob(string userId)
        {
            lock (cloudAppendBlobDictionary)
            {
                CloudAppendBlob cloudAppendBlob =
                    this.container.GetAppendBlobReference($"{userId}_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture)}.log");
                if (!cloudAppendBlob.Exists())
                {
                    cloudAppendBlob.CreateOrReplace();
                }
                cloudAppendBlobDictionary[userId] = cloudAppendBlob;
                return cloudAppendBlobDictionary[userId];
            }
        }

        private async Task SendMessageAsync(string userId, Uri blobUri)
        {
            using (BrokeredMessage message = new BrokeredMessage(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(
                        new UserSession
                        {
                            UserId = userId,
                            Uri = blobUri
                        }))))
            {
                await this.queueClient.SendAsync(message);
            }
        }

        #endregion
    }
}