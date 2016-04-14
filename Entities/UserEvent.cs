// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.Entities
{
    using System;
    using Newtonsoft.Json;

    public class UserEvent
    {
        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        [JsonProperty(PropertyName = "type", Order = 1)]
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the event timestamp.
        /// </summary>
        [JsonProperty(PropertyName = "timestamp", Order = 2)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the mouse position.
        /// </summary>
        [JsonProperty(PropertyName = "position", Order = 3)]
        public Position MousePosition { get; set; }

        /// <summary>
        /// Gets or sets the mouse position.
        /// </summary>
        [JsonProperty(PropertyName = "text", Order = 4)]
        public string EnteredText { get; set; }
    }
}