using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense
{
    public class WithheldInfoV2
    {
        /// <summary>
        /// Provides a list of countries where this content is not available.
        /// </summary>
        [JsonProperty("country_codes")] public string[] CountryCodes { get; set; }

        /// <summary>
        /// Indicates whether the content being withheld is a Tweet or a user.
        /// </summary>
        [JsonProperty("scope")] public string Scope { get; set; }
    }
}
