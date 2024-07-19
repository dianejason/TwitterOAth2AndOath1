using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Respense
{
    public class AccessTokenResponseV1
    {
        public string oauth_token { get; set; }
        public string oauth_token_secret { get; set; }
        public string user_id { get; set; }
        public string screen_name { get; set; }
    }
}
