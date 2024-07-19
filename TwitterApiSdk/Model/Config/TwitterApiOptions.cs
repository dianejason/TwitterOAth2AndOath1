using System;

namespace TwitterApiSdk
{
    public class TwitterApiOptions
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string ApiHost { get; set; }
        public string AuthHost { get; set; }
        public string UploadHost { get; set; }
        public string Scopes { get; set; }
        public string CallbackUrl { get; set; }
        public string ProxyServerUrl { get; set; }

        /// <summary>
        /// OAuth1.0
        /// </summary>
        public string ApiKey { get; set; }
        public string ApiKeySecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}
