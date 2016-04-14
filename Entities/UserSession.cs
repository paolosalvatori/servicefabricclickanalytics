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

    public class UserSession
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [JsonProperty(PropertyName = "id", Order = 1)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the uri of the append blob containing the user session.
        /// </summary>
        [JsonProperty(PropertyName = "uri", Order = 2)]
        public Uri Uri { get; set; }
    }
}