using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense
{
    public class UserEntitiesV2
    {
        /// <summary>
        /// Contains details about URLs, Hashtags, Cashtags, or mentions located within a user's description.
        /// </summary>
        [JsonProperty("description")] public dynamic Description { get; set; }

        /// <summary>
        /// Contains details about the user's profile website.
        /// </summary>
        [JsonProperty("url")] public dynamic Url { get; set; }
    }
}
