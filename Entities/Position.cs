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

using Newtonsoft.Json;

#endregion

namespace Microsoft.AzureCat.Samples.Entities
{
    public class Position
    {
        /// <summary>
        ///     Gets or sets x coordinate.
        /// </summary>
        [JsonProperty(PropertyName = "x", Order = 1)]
        public int X { get; set; }

        /// <summary>
        ///     Gets or sets y coordinate.
        /// </summary>
        [JsonProperty(PropertyName = "y", Order = 2)]
        public int Y { get; set; }
    }
}