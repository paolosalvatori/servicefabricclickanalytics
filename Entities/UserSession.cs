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
    public class UserSession
    {
        /// <summary>
        ///     Gets or sets the user id.
        /// </summary>
        [JsonProperty(PropertyName = "id", Order = 1)]
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the uri of the append blob containing the user session.
        /// </summary>
        [JsonProperty(PropertyName = "uri", Order = 2)]
        public Uri Uri { get; set; }
    }
}