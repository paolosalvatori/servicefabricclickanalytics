// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.EventProcessorHostService
{
    using System;
    using Microsoft.ServiceBus.Messaging;

    public class EventProcessorFactory<T> : IEventProcessorFactory where T : class, IEventProcessor
    {
        #region IEventProcessorFactory Methods

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return this.instance ??
                   Activator.CreateInstance(
                       typeof(T),
                       this.serviceBusConnectionString,
                       this.storageAccountConnectionString,
                       this.containerName,
                       this.queueName,
                       this.checkpointCount) as T;
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
            this.storageAccountConnectionString = null;
            this.containerName = null;
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