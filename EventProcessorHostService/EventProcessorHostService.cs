// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.AzureCat.Samples.EventProcessorHostService
{
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Fabric.Description;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;

    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class EventProcessorHostService : StatelessService
    {
        #region Public Constructor

        public EventProcessorHostService(StatelessServiceContext context)
            : base(context)
        {
        }

        #endregion

        #region Private Methods

        private void ReadSettings()
        {
            // Read settings from the DeviceActorServiceConfig section in the Settings.xml file
            ICodePackageActivationContext activationContext = this.Context.CodePackageActivationContext;
            ConfigurationPackage config = activationContext.GetConfigurationPackageObject(ConfigurationPackage);
            ConfigurationSection section = config.Settings.Sections[ConfigurationSection];

            // Read the MaxRetryCount setting from the Settings.xml file
            this.maxRetryCount = DefaultMaxRetryCount;
            ConfigurationProperty parameter = section.Parameters[MaxQueryRetryCountParameter];
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

        #endregion

        #region Private Constants

        //************************************
        // Parameters
        //************************************
        private const string ConfigurationPackage = "Config";
        private const string ConfigurationSection = "EventProcessorHostConfig";
        private const string MaxQueryRetryCountParameter = "MaxRetryCount";
        private const string BackoffDelayParameter = "BackoffDelay";

        //************************************
        // Formats & Messages
        //************************************
        private const string RetryTimeoutExhausted = "Retry timeout exhausted.";

        //************************************
        // Constants
        //************************************
        private const int DefaultMaxRetryCount = 3;
        private const int DefaultBackoffDelay = 1;

        #endregion

        #region Private Fields

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
                    return new[]
                    {
                        new ServiceInstanceListener(s => new EventProcessorHostListener(s))
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
    }
}