#region Copyright

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
using Newtonsoft.Json;

#endregion

namespace Microsoft.AzureCat.Samples.Entities
{
    public class UserEvent
    {
        /// <summary>
        ///     Gets or sets the event type.
        /// </summary>
        [JsonProperty(PropertyName = "type", Order = 1)]
        public EventType EventType { get; set; }

        /// <summary>
        ///     Gets or sets the event timestamp.
        /// </summary>
        [JsonProperty(PropertyName = "timestamp", Order = 2)]
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///     Gets or sets the mouse position.
        /// </summary>
        [JsonProperty(PropertyName = "position", Order = 3)]
        public Position MousePosition { get; set; }

        /// <summary>
        ///     Gets or sets the mouse position.
        /// </summary>
        [JsonProperty(PropertyName = "text", Order = 4)]
        public string EnteredText { get; set; }
    }
}