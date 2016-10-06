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
using Microsoft.ServiceBus.Messaging;

#endregion

namespace Microsoft.AzureCat.Samples.EventProcessorHostService
{
    public class EventProcessorFactory<T> : IEventProcessorFactory where T : class, IEventProcessor
    {
        #region IEventProcessorFactory Methods

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return instance ??
                   Activator.CreateInstance(
                       typeof(T),
                       serviceBusConnectionString,
                       storageAccountConnectionString,
                       containerName,
                       queueName,
                       checkpointCount) as T;
        }

        #endregion

        #region Private Fields

        private readonly T instance;
        private readonly string serviceBusConnectionString;
        private readonly string storageAccountConnectionString;
        private readonly string containerName;
        private readonly string queueName;
        private readonly int checkpointCount = 100;

        #endregion

        #region Public Constructors

        public EventProcessorFactory()
        {
            storageAccountConnectionString = null;
            containerName = null;
        }

        public EventProcessorFactory(
            string serviceBusConnectionString,
            string storageAccountConnectionString,
            string containerName,
            string queueName,
            int checkpointCount)
        {
            this.serviceBusConnectionString = serviceBusConnectionString;
            this.storageAccountConnectionString = storageAccountConnectionString;
            this.containerName = containerName;
            this.queueName = queueName;
            this.checkpointCount = checkpointCount;
        }

        public EventProcessorFactory(T instance)
        {
            this.instance = instance;
        }

        #endregion
    }
}