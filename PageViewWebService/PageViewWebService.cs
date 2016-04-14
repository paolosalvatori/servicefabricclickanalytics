// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.PageViewWebService
{
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Fabric.Description;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;

    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class PageViewWebService : StatelessService
    {
        #region Public Constructor

        public PageViewWebService(StatelessServiceContext context)
            : base(context)
        {
        }

        #endregion

        #region Private Constants

        //************************************
        // Parameters
        //************************************
        private const string ConfigurationPackage = "Config";
        private const string ConfigurationSection = "PageViewWebServiceConfig";
        private const string ServiceBusConnectionStringParameter = "ServiceBusConnectionString";
        private const string EventHubNameParameter = "EventHubName";
        private const string EventHubClientNumberParameter = "EventHubClientNumber";
        private const string MaxQueryRetryCountParameter = "MaxRetryCount";
        private const string BackoffDelayParameter = "BackoffDelay";

        //************************************
        // Formats & Messages
        //************************************
        private const string ParameterCannotBeNullFormat = "The parameter [{0}] is not defined in the Setting.xml configuration file.";
        private const string RetryTimeoutExhausted = "Retry timeout exhausted.";

        //************************************
        // Constants
        //************************************
        private const int DefaultEventHubClientNumber = 32;
        private const int DefaultMaxRetryCount = 3;
        private const int DefaultBackoffDelay = 1;

        #endregion

        #region Private Fields

        private string serviceBusConnectionString;
        private string eventHubName;
        private int eventHubClientNumber;
        private int maxRetryCount;
        private int backoffDelay;

        #endregion

        #region StatelessService Protected Methods

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            this.ReadSettings();
            for (int k = 1; k <= this.maxRetryCount; k++)
            {
                try
                {
                    for (int i = 1; i < this.eventHubClientNumber; i++)
                    {
                        EventHubClient eventHubClient = this.CreateEventHubClient();
                        if (eventHubClient != null)
                        {
                            PageViewController.EventHubClientList.Add(eventHubClient);
                        }
                        else
                        {
                            break;
                        }
                    }
                    return new[]
                    {
                        new ServiceInstanceListener(s => new OwinCommunicationListener("usersessions", new Startup(), s))
                    };
                }
                catch (FabricTransientException ex)
                {
                    ServiceEventSource.Current.Message(ex.Message);
                }
                catch (AggregateException ex)
                {
                    foreach (Exception e in ex.InnerExceptions)
                    {
                        ServiceEventSource.Current.Message(e.Message);
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.Message(ex.Message);
                    throw;
                }
                Task.Delay(this.backoffDelay).Wait();
            }
            throw new TimeoutException(RetryTimeoutExhausted);
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancelServiceInstance">Canceled when Service Fabric terminates this instance.</param>
        protected override async Task RunAsync(CancellationToken cancelServiceInstance)
        {
            // This service instance continues processing until the instance is terminated.
            while (!cancelServiceInstance.IsCancellationRequested)
            {
                // Pause for 1 second before continue processing.
                await Task.Delay(TimeSpan.FromSeconds(1), cancelServiceInstance);
            }
        }

        #endregion

        #region Private Methods

        private void ReadSettings()
        {
            // Read settings from the DeviceActorServiceConfig section in the Settings.xml file
            ICodePackageActivationContext activationContext = this.Context.CodePackageActivationContext;
            ConfigurationPackage config = activationContext.GetConfigurationPackageObject(ConfigurationPackage);
            ConfigurationSection section = config.Settings.Sections[ConfigurationSection];

            // Read the ServiceBusConnectionString setting from the Settings.xml file
            ConfigurationProperty parameter = section.Parameters[ServiceBusConnectionStringParameter];
            if (!string.IsNullOrWhiteSpace(parameter?.Value))
            {
                this.serviceBusConnectionString = parameter.Value;
            }
            else
            {
                throw new ArgumentException(
                    string.Format(ParameterCannotBeNullFormat, ServiceBusConnectionStringParameter),
                    ServiceBusConnectionStringParameter);
            }

            // Read the EventHubName setting from the Settings.xml file
            parameter = section.Parameters[EventHubNameParameter];
            if (!string.IsNullOrWhiteSpace(parameter?.Value))
            {
                this.eventHubName = parameter.Value;
            }
            else
            {
                throw new ArgumentException(
                    string.Format(ParameterCannotBeNullFormat, EventHubNameParameter),
                    EventHubNameParameter);
            }

            // Read the EventHubClientNumber setting from the Settings.xml file
            this.eventHubClientNumber = DefaultEventHubClientNumber;
            parameter = section.Parameters[EventHubClientNumberParameter];
            if (!string.IsNullOrWhiteSpace(parameter?.Value))
            {
                int.TryParse(parameter.Value, out this.eventHubClientNumber);
            }

            // Read the MaxRetryCount setting from the Settings.xml file
            this.maxRetryCount = DefaultMaxRetryCount;
            parameter = section.Parameters[MaxQueryRetryCountParameter];
            if (!string.IsNullOrWhiteSpace(parameter?.Value))
            {
                int.TryParse(parameter.Value, out this.maxRetryCount);
            }

            // Read the BackoffDelay setting from the Settings.xml file
            this.backoffDelay = DefaultBackoffDelay;
            parameter = section.Parameters[BackoffDelayParameter];
            if (!string.IsNullOrWhiteSpace(parameter?.Value))
            {
                int.TryParse(parameter.Value, out this.backoffDelay);
            }
        }

        public EventHubClient CreateEventHubClient()
        {
            if (string.IsNullOrWhiteSpace(this.serviceBusConnectionString) || string.IsNullOrWhiteSpace(this.eventHubName))
            {
                return null;
            }
            MessagingFactory messagingFactory =
                MessagingFactory.CreateFromConnectionString(
                    new ServiceBusConnectionStringBuilder(this.serviceBusConnectionString)
                    {
                        TransportType = TransportType.Amqp
                    }.ToString());
            return messagingFactory.CreateEventHubClient(this.eventHubName);
        }

        #endregion
    }
}