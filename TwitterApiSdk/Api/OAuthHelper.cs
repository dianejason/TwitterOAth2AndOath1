using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TwitterApiSdk.Api
{
    public static class OAuthHelper
    {
        public static string GenerateOAuthHeader(string httpMethod, string url, string apiKey, string apiSecretKey, string token = "", string tokenSecret = "", Dictionary<string, string> additionalParams = null)
        {
            var oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var oauthTimestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            var oauthParams = new Dictionary<string, string>
        {
            { "oauth_consumer_key", apiKey },
            { "oauth_nonce", oauthNonce },
            { "oauth_signature_method", "HMAC-SHA1" },
            { "oauth_timestamp", oauthTimestamp },
            { "oauth_version", "1.0" }
        };

            if (!string.IsNullOrEmpty(token))
            {
                oauthParams.Add("oauth_token", token);
            }

            if (additionalParams != null)
            {
                foreach (var param in additionalParams)
                {
                    oauthParams.Add(param.Key, param.Value);
                }
            }

            var baseString = GenerateBaseString(httpMethod, url, oauthParams);
            var compositeKey = $"{Uri.EscapeDataString(apiSecretKey)}&{Uri.EscapeDataString(tokenSecret)}";
            var oauthSignature = GenerateSignature(baseString, compositeKey);

            oauthParams.Add("oauth_signature", oauthSignature);

            var authHeader = "OAuth " + string.Join(", ", oauthParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}=\"{Uri.EscapeDataString(kvp.Value)}\""));
            return authHeader;
        }

        private static string GenerateBaseString(string httpMethod, string url, Dictionary<string, string> oauthParams)
        {
            var sortedParams = oauthParams.OrderBy(kvp => kvp.Key).ThenBy(kvp => kvp.Value);
            var paramString = string.Join("&", sortedParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

            var baseString = $"{httpMethod.ToUpper()}&{Uri.EscapeDataString(url)}&{Uri.EscapeDataString(paramString)}";
            return baseString;
        }

        private static string GenerateSignature(string baseString, string compositeKey)
        {
            using (var hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(compositeKey)))
            {
                var hashBytes = hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
