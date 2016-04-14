// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.EventProcessorHostService
{
    using System;
    using System.Fabric;
    using System.Fabric.Description;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class EventProcessorHostListener : ICommunicationListener
    {
        #region Public Constructor

        public EventProcessorHostListener(StatelessServiceContext context)
        {
            this.context = context;
        }

        #endregion

        #region Private Constants

        //************************************
        // Parameters
        //************************************
        private const string ConfigurationPackage = "Config";
        private const string ConfigurationSection = "EventProcessorHostConfig";
        private const string StorageAccountConnectionStringParameter = "StorageAccountConnectionString";
        private const string ServiceBusConnectionStringParameter = "ServiceBusConnectionString";
        private const string EventHubNameParameter = "EventHubName";
        private const string ConsumerGroupNameParameter = "ConsumerGroupName";
        private const string ContainerNameParameter = "ContainerName";
        private const string QueueNameParameter = "QueueName";
        private const string CheckpointCountParameter = "CheckpointCount";

        //************************************
        // Formats
        //************************************
        private const string ParameterCannotBeNullFormat = "The parameter [{0}] is not defined in the Setting.xml configuration file.";
        private const string RegisteringEventProcessor = "Registering Event Processor [EventProcessor]... ";
        private const string EventProcessorRegistered = "Event Processor [EventProcessor] successfully registered. ";

        #endregion

        #region Private Fields

        private string storageAccountConnectionString;
        private string serviceBusConnectionString;
        private string eventHubName;
        private string consumerGroupName;
        private string containerName;
        private string queueName;
        private int checkpointCount = 100;
        private EventProcessorHost eventProcessorHost;
        private readonly StatelessServiceContext context;

        #endregion

        #region ICommunicationListener Methods

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Get the EventProcessorHostConfig section from the Settings.xml file
                ICodePackageActivationContext codePackageActivationContext = this.context.CodePackageActivationContext;
                ConfigurationPackage config = codePackageActivationContext.GetConfigurationPackageObject(ConfigurationPackage);
                ConfigurationSection section = config.Settings.Sections[ConfigurationSection];

                // Check if a parameter called ServiceBusConnectionString exists in the EventProcessorHostConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                        p.Name,
                        StorageAccountConnectionStringParameter,
                        StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // Read the StorageAccountConnectionString setting from the Settings.xml file
                    ConfigurationProperty parameter = section.Parameters[StorageAccountConnectionStringParameter];
                    if (!string.IsNullOrWhiteSpace(parameter?.Value))
                    {
                        this.storageAccountConnectionString = parameter.Value;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format(
                                ParameterCannotBeNullFormat,
                                StorageAccountConnectionStringParameter),
                            StorageAccountConnectionStringParameter);
                    }
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            ParameterCannotBeNullFormat,
                            StorageAccountConnectionStringParameter),
                        StorageAccountConnectionStringParameter);
                }

                // Check if a parameter called ServiceBusConnectionString exists in the EventProcessorHostConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                        p.Name,
                        ServiceBusConnectionStringParameter,
                        StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // Read the ServiceBusConnectionString setting from the Settings.xml file
                    ConfigurationProperty parameter = section.Parameters[ServiceBusConnectionStringParameter];
                    if (!string.IsNullOrWhiteSpace(parameter?.Value))
                    {
                        this.serviceBusConnectionString = parameter.Value;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format(
                                ParameterCannotBeNullFormat,
                                ServiceBusConnectionStringParameter),
                            ServiceBusConnectionStringParameter);
                    }
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            ParameterCannotBeNullFormat,
                            ServiceBusConnectionStringParameter),
                        ServiceBusConnectionStringParameter);
                }

                // Check if a parameter called ConsumerGroupName exists in the EventProcessorHostConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                        p.Name,
                        ConsumerGroupNameParameter,
                        StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // Read the ConsumerGroupName setting from the Settings.xml file
                    ConfigurationProperty parameter = section.Parameters[ConsumerGroupNameParameter];
                    if (!string.IsNullOrWhiteSpace(parameter?.Value))
                    {
                        this.consumerGroupName = parameter.Value;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format(
                                ParameterCannotBeNullFormat,
                                ConsumerGroupNameParameter),
                            ConsumerGroupNameParameter);
                    }
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            ParameterCannotBeNullFormat,
                            ConsumerGroupNameParameter),
                        ConsumerGroupNameParameter);
                }

                // Check if a parameter called ContainerName exists in the EventProcessorHostConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                        p.Name,
                        ContainerNameParameter,
                        StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // Read the ContainerName setting from the Settings.xml file
                    ConfigurationProperty parameter = section.Parameters[ContainerNameParameter];
                    if (!string.IsNullOrWhiteSpace(parameter?.Value))
                    {
                        this.containerName = parameter.Value;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format(
                                ParameterCannotBeNullFormat,
                                ContainerNameParameter),
                            ContainerNameParameter);
                    }
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            ParameterCannotBeNullFormat,
                            ContainerNameParameter),
                        ContainerNameParameter);
                }

                // Check if a parameter called QueueName exists in the EventProcessorHostConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                        p.Name,
                        QueueNameParameter,
                        StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // Read the QueueName setting from the Settings.xml file
                    ConfigurationProperty parameter = section.Parameters[QueueNameParameter];
                    if (!string.IsNullOrWhiteSpace(parameter?.Value))
                    {
                        this.queueName = parameter.Value;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format(
                                ParameterCannotBeNullFormat,
                                QueueNameParameter),
                            QueueNameParameter);
                    }
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            ParameterCannotBeNullFormat,
                            QueueNameParameter),
                        QueueNameParameter);
                }

                // Check if a parameter called EventHubName exists in the EventProcessorHostConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                        p.Name,
                        EventHubNameParameter,
                        StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // Read the EventHubName setting from the Settings.xml file
                    ConfigurationProperty parameter = section.Parameters[EventHubNameParameter];
                    if (!string.IsNullOrWhiteSpace(parameter?.Value))
                    {
                        this.eventHubName = parameter.Value;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format(
                                ParameterCannotBeNullFormat,
                                EventHubNameParameter),
                            EventHubNameParameter);
                    }
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            ParameterCannotBeNullFormat,
                            EventHubNameParameter),
                        EventHubNameParameter);
                }

                // Check if a parameter called CheckpointCount exists in the EventProcessorHostConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                        p.Name,
                        CheckpointCountParameter,
                        StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    // Read the CheckpointCount setting from the Settings.xml file
                    ConfigurationProperty parameter = section.Parameters[CheckpointCountParameter];
                    int value;
                    if (int.TryParse(parameter.Value, out value))
                    {
                        this.checkpointCount = value;
                    }
                }

                // Start EventProcessorHost
                await this.StartEventProcessorAsync();

                // Return Event Hub name
                return this.eventHubName;
            }
            catch (Exception ex)
            {
                // Trace Error
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                // Trace Error
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        public void Abort()
        {
            try
            {
            }
            catch (Exception ex)
            {
                // Trace Error
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        #endregion

        #region Private Methods

        private async Task StartEventProcessorAsync()
        {
            try
            {
                // Create CloudStorageAccount instance
                CloudStorageAccount cloudStorageAccount;
                CloudStorageAccount.TryParse(this.storageAccountConnectionString, out cloudStorageAccount);

                //Create service client for credentialed access to the Blob service.
                CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

                //Get a reference to a container.
                CloudBlobContainer container = blobClient.GetContainerReference(this.containerName);

                //Create the container if it does not already exist.
                container.CreateIfNotExists();

                // Create EventHubClient instance
                EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(this.serviceBusConnectionString, this.eventHubName);

                // Get the default Consumer Group
                this.eventProcessorHost = new EventProcessorHost(
                    Guid.NewGuid().ToString(),
                    eventHubClient.Path.ToLower(),
                    this.consumerGroupName.ToLower(),
                    this.serviceBusConnectionString,
                    this.storageAccountConnectionString)
                {
                    PartitionManagerOptions = new PartitionManagerOptions
                    {
                        AcquireInterval = TimeSpan.FromSeconds(10), // Default is 10 seconds
                        RenewInterval = TimeSpan.FromSeconds(10), // Default is 10 seconds
                        LeaseInterval = TimeSpan.FromSeconds(30) // Default value is 30 seconds
                    }
                };
                ServiceEventSource.Current.Message(RegisteringEventProcessor);
                EventProcessorOptions eventProcessorOptions = new EventProcessorOptions
                {
                    InvokeProcessorAfterReceiveTimeout = true,
                    MaxBatchSize = 100,
                    PrefetchCount = 100,
                    ReceiveTimeOut = TimeSpan.FromSeconds(30),
                };
                eventProcessorOptions.ExceptionReceived += EventProcessorOptions_ExceptionReceived;
                await
                    this.eventProcessorHost.RegisterEventProcessorFactoryAsync(
                        new EventProcessorFactory<EventProcessor>(
                            this.serviceBusConnectionString,
                            this.storageAccountConnectionString,
                            this.containerName,
                            this.queueName,
                            this.checkpointCount),
                        eventProcessorOptions);
                ServiceEventSource.Current.Message(EventProcessorRegistered);
            }
            catch (Exception ex)
            {
                // Trace Error
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        private static void EventProcessorOptions_ExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            if (e?.Exception == null)
            {
                return;
            }

            // Trace Exception
            ServiceEventSource.Current.Message(e.Exception.Message, e.Exception.InnerException?.Message ?? string.Empty);
        }

        #endregion
    }
}