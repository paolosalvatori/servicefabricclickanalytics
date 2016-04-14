// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.Entities
{
    using Newtonsoft.Json;

    public class Position
    {
        /// <summary>
        /// Gets or sets x coordinate.
        /// </summary>
        [JsonProperty(PropertyName = "x", Order = 1)]
        public int X { get; set; }

        /// <summary>
        /// Gets or sets y coordinate.
        /// </summary>
        [JsonProperty(PropertyName = "y", Order = 2)]
        public int Y { get; set; }
    }
}