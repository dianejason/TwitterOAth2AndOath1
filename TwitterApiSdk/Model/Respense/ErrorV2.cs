﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense
{
    public class ErrorV2
    {
        // client errors
        [JsonProperty("client_id")] public string ClientId { get; set; }
        [JsonProperty("required_enrollment")] public string RequiredEnrollment { get; set; }
        [JsonProperty("registration_url")] public string RegistrationUrl { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("detail")] public string Detail { get; set; }
        [JsonProperty("reason")] public string Reason { get; set; }
        [JsonProperty("type")] public string Type { get; set; }

        // parameters error
        [JsonProperty("resource_type")] public string ResourceType { get; set; }
        [JsonProperty("parameter")] public string Parameter { get; set; }
        [JsonProperty("value")] public string Value { get; set; }
    }
}
