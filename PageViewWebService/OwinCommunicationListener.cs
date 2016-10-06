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
using System.Fabric;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

#endregion

namespace Microsoft.AzureCat.Samples.PageViewWebService
{
    public class OwinCommunicationListener : ICommunicationListener
    {
        #region Public Constructor

        public OwinCommunicationListener(string appRoot, IOwinAppBuilder startup, StatelessServiceContext context)
        {
            this.startup = startup;
            this.appRoot = appRoot;
            this.context = context;
        }

        #endregion

        #region Public Static Properties

        public static string DeviceActorServiceUri { get; private set; }

        #endregion

        #region Private Methods

        private void StopWebServer()
        {
            if (serverHandle == null)
                return;
            try
            {
                serverHandle.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // no-op
            }
        }

        #endregion

        #region Private Constants

        //************************************
        // Parameters
        //************************************
        private const string ConfigurationPackage = "Config";
        private const string ConfigurationSection = "PageViewWebServiceConfig";
        private const string DeviceActorServiceUriParameter = "DeviceActorServiceUri";

        #endregion

        #region Private Fields

        private readonly IOwinAppBuilder startup;
        private readonly string appRoot;
        private readonly StatelessServiceContext context;
        private IDisposable serverHandle;
        private string listeningAddress;

        #endregion

        #region ICommunicationListener Methods

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Read settings from the DeviceActorServiceConfig section in the Settings.xml file
                var codePackageActivationContext = context.CodePackageActivationContext;
                var config = codePackageActivationContext.GetConfigurationPackageObject(ConfigurationPackage);
                var section = config.Settings.Sections[ConfigurationSection];

                // Check if a parameter called DeviceActorServiceUri exists in the DeviceActorServiceConfig config section
                if (section.Parameters.Any(
                    p => string.Compare(
                             p.Name,
                             DeviceActorServiceUriParameter,
                             StringComparison.InvariantCultureIgnoreCase) == 0))
                {
                    var parameter = section.Parameters[DeviceActorServiceUriParameter];
                    DeviceActorServiceUri = !string.IsNullOrWhiteSpace(parameter?.Value)
                        ? parameter.Value
                        :
                        // By default, the current service assumes that if no URI is explicitly defined for the actor service
                        // in the Setting.xml file, the latter is hosted in the same Service Fabric application.
                        $"fabric:/{context.ServiceName.Segments[1]}DeviceActorService";
                }
                else
                {
                    // By default, the current service assumes that if no URI is explicitly defined for the actor service
                    // in the Setting.xml file, the latter is hosted in the same Service Fabric application.
                    DeviceActorServiceUri = $"fabric:/{context.ServiceName.Segments[1]}DeviceActorService";
                }

                var serviceEndpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
                var port = serviceEndpoint.Port;

                listeningAddress = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://+:{0}/{1}",
                    port,
                    string.IsNullOrWhiteSpace(appRoot)
                        ? string.Empty
                        : appRoot.TrimEnd('/') + '/');

                serverHandle = WebApp.Start(listeningAddress, appBuilder => startup.Configuration(appBuilder));
                var publishAddress = listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);

                ServiceEventSource.Current.Message($"OWIN listening on [{publishAddress}]");

                return Task.FromResult(publishAddress);
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message(ex.Message);
                throw;
            }
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            ServiceEventSource.Current.Message("Close");

            StopWebServer();

            return Task.FromResult(true);
        }

        public void Abort()
        {
            ServiceEventSource.Current.Message("Abort");

            StopWebServer();
        }

        #endregion
    }
}