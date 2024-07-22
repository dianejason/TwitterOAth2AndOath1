using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterApiSdk.Model.Config
{
    public class ConstRouter
    {
        public const string AuthorizeV2 = "/i/oauth2/authorize";
        public const string RefreshTokenv2 = "/2/oauth2/token";
        public const string Revokev2 = "/2/oauth2/revoke";
        public const string MeV2 = "/2/users/me";
        public const string GetUserByIdV2 = "/2/users/";
        public const string TweetsV2 = "/2/tweets";
        public const string UploadV1 = "/1.1/media/upload.json";
        public const string UploadV1FullUrl = "https://upload.twitter.com/1.1/media/upload.json";

        public const string RequestToken = "/oauth/request_token";
        public const string UserLookupV1 = "/1.1/users/lookup.json";
    }
}
