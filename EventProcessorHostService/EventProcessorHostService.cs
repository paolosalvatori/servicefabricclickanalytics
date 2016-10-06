﻿#region Copyright

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
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

#endregion

namespace Microsoft.AzureCat.Samples.EventProcessorHostService
{
    /// <summary>
    ///     The FabricRuntime creates an instance of this class for each service type instance.
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
            var activationContext = Context.CodePackageActivationContext;
            var config = activationContext.GetConfigurationPackageObject(ConfigurationPackage);
            var section = config.Settings.Sections[ConfigurationSection];

            // Read the MaxRetryCount setting from the Settings.xml file
            maxRetryCount = DefaultMaxRetryCount;
            var parameter = section.Parameters[MaxQueryRetryCountParameter];
            if (!string.IsNullOrWhiteSpace(parameter?.Value))
                int.TryParse(parameter.Value, out maxRetryCount);

            // Read the BackoffDelay setting from the Settings.xml file
            backoffDelay = DefaultBackoffDelay;
            parameter = section.Parameters[BackoffDelayParameter];
            if (!string.IsNullOrWhiteSpace(parameter?.Value))
                int.TryParse(parameter.Value, out backoffDelay);
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
        ///     Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            ReadSettings();
            for (var k = 1; k <= maxRetryCount; k++)
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
                    foreach (var e in ex.InnerExceptions)
                        ServiceEventSource.Current.Message(e.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.Message(ex.Message);
                    throw;
                }
                Task.Delay(backoffDelay).Wait();
            }
            throw new TimeoutException(RetryTimeoutExhausted);
        }

        /// <summary>
        ///     This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancelServiceInstance">Canceled when Service Fabric terminates this instance.</param>
        protected override async Task RunAsync(CancellationToken cancelServiceInstance)
        {
            // This service instance continues processing until the instance is terminated.
            while (!cancelServiceInstance.IsCancellationRequested)
                await Task.Delay(TimeSpan.FromSeconds(1), cancelServiceInstance);
        }

        #endregion
    }
}