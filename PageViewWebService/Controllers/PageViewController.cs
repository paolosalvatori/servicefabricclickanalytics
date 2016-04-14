// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directices



#endregion

namespace Microsoft.AzureCat.Samples.PageViewWebService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.AzureCat.Samples.Entities;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;

    public class PageViewController : ApiController
    {
        #region Internal Static Fields

        internal static readonly List<EventHubClient> EventHubClientList = new List<EventHubClient>();

        #endregion

        #region Private Static Fields

        private static readonly Random random = new Random();

        #endregion

        #region Private Static Methods

        private static EventHubClient GetEventHubClient()
        {
            return EventHubClientList[random.Next(0, EventHubClientList.Count)];
        }

        #endregion

        #region Private Constants

        //************************************
        // Headers
        //************************************
        private const string UserIdHeader = "userId";

        //************************************
        // Properties
        //************************************
        private const string UserIdProperty = "userId";
        private const string EventTypeProperty = "eventType";

        #endregion

        #region Public Methods

        [HttpGet]
        public string Test()
        {
            return "abracadabra";
        }

        [HttpPost]
        public async Task SendPayload(Payload payload)
        {
            try
            {
                // Validates input
                if (payload?.UserEvent == null)
                {
                    return;
                }

                // Gets the userid from the payload or from the header
                string userId;
                if (!string.IsNullOrWhiteSpace(payload.UserId))
                {
                    userId = payload.UserId;
                }
                else
                {
                    IEnumerable<string> headerValues;
                    if (!this.Request.Headers.TryGetValues(UserIdHeader, out headerValues))
                    {
                        return;
                    }
                    userId = headerValues.FirstOrDefault();
                }
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return;
                }

                // Gets an EventHubClient from the pool
                EventHubClient eventHubClient = GetEventHubClient();
                if (eventHubClient == null)
                {
                    return;
                }

                // Submits the UserEvent to the EventHub
                using (EventData eventData = new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload.UserEvent)))
                {
                    // Uses the userId as partition key. This way all the events from a user session 
                    // will end up in the same partition and processed in a chronological order.
                    PartitionKey = userId
                })
                {
                    // Adds the userId as a custom property
                    eventData.Properties.Add(UserIdProperty, userId);

                    // Adds the event type as a custom property
                    eventData.Properties.Add(EventTypeProperty, (int) payload.UserEvent.EventType);

                    // Sends the event to the event hub.
                    // Note: consider removing the await statement to avoid waiting for the completion
                    // of the send operation in case you want to reduce the latency
                    await eventHubClient.SendAsync(eventData);

                    ServiceEventSource.Current.Message($"Event sent: EventHub=[{eventHubClient.Path}] UserId=[{userId}]");
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions?.Count > 0)
                {
                    foreach (Exception exception in ex.InnerExceptions)
                    {
                        ServiceEventSource.Current.Message(exception.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.Message(ex.Message);
            }
        }

        #endregion
    }
}