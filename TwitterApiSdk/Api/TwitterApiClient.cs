using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using Samm.OpenApi.Adapter;
using Samm.OpenApi.Adapter.Http;
using TwitterApiSdk.Model.Config;
using TwitterApiSdk.Model.Respense;

namespace TwitterApiSdk.Api
{
    public class TwitterApiClient : ITwitterApiClient
    {
        private readonly IHttpClient _httpClient;
        private readonly TwitterApiOptions _twitterApiOptions;
        public TwitterApiClient(IHttpClient httpClient,
            IOptions<TwitterApiOptions> options)
        {
            _httpClient = httpClient;
            _twitterApiOptions = options.Value;
            _httpClient.SetHost(_twitterApiOptions.ApiHost);
        }

        public async Task<AccessTokenResponse> AccessTokenV2(string code, string challenge)
        {
            ArgumentCheck.Begin().NotNull(code, "code");
            ArgumentCheck.Begin().NotNull(challenge, "challenge");
            _httpClient.SetApiInfo(ConstRouter.RefreshTokenv2, HttpMethod.Post);
            var apiParameter = new ApiParameter();
            var basicAuthorization = ($"{_twitterApiOptions.AppId}:{_twitterApiOptions.AppSecret}").ToBase64();
            apiParameter.AddToHeader("Authorization", "Basic " + basicAuthorization);
            apiParameter.AddToFormUrlData("code", code);
            apiParameter.AddToFormUrlData("grant_type", "authorization_code");
            apiParameter.AddToFormUrlData("client_id", _twitterApiOptions.AppId);
            apiParameter.AddToFormUrlData("redirect_uri", _twitterApiOptions.CallbackUrl);
            apiParameter.AddToFormUrlData("code_verifier", challenge);

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<AccessTokenResponse>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public string ConnectV2(string state, string codeChallenge)
        {
            var scope = HttpUtility.UrlEncode(_twitterApiOptions.Scopes);
            return $"{_twitterApiOptions.AuthHost}{ConstRouter.AuthorizeV2}?response_type=code&client_id={_twitterApiOptions.AppId}&redirect_uri={_twitterApiOptions.CallbackUrl}&scope={scope}&state={state}&code_challenge={codeChallenge}&code_challenge_method=plain";
        }

        public async Task<AccessTokenResponse> RefreshTokenV2(string refreshToken)
        {
            ArgumentCheck.Begin().NotNull(refreshToken, "refreshToken");
            _httpClient.SetApiInfo(ConstRouter.RefreshTokenv2, HttpMethod.Post);
            var apiParameter = new ApiParameter();
            var basicAuthorization = ($"{_twitterApiOptions.AppId}:{_twitterApiOptions.AppSecret}").ToBase64();
            apiParameter.AddToHeader("Authorization", "Basic " + basicAuthorization);
            apiParameter.AddToFormUrlData("grant_type", "refresh_token");
            apiParameter.AddToFormUrlData("client_id", _twitterApiOptions.AppId);
            apiParameter.AddToFormUrlData("refresh_token", refreshToken);
            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<AccessTokenResponse>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<dynamic> RevokeV2(string accessToken)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            _httpClient.SetApiInfo(ConstRouter.Revokev2, HttpMethod.Post);
            var apiParameter = new ApiParameter();
            apiParameter.AddToFormUrlData("client_id", _twitterApiOptions.AppId);
            apiParameter.AddToFormUrlData("token", accessToken);

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<dynamic>(result.Data);
            }
            else
            {
                return null;
            }
        }

        private void AddAuthorization(ApiParameter apiParameter, string accessToken)
        {
            apiParameter.AddToHeader("Authorization", "Bearer " + accessToken);
        }
        public async Task<UserV2Response> GetCurrentUserV2(string accessToken, HashSet<string> userFields = null, HashSet<string> tweetFields = null, HashSet<string> expansions = null)
        {
            if (userFields == null)
            {
                userFields = UserResponseFields.User.ALL;
            }
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            _httpClient.SetApiInfo(ConstRouter.MeV2, HttpMethod.Get);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, accessToken);
            apiParameter.AddToFormUrlData("user.fields", string.Join(",", userFields));
            if (tweetFields != null && tweetFields.Any())
            {
                apiParameter.AddToFormUrlData("tweet.fields", string.Join(",", tweetFields));
            }
            if (expansions != null && expansions.Any())
            {
                apiParameter.AddToFormUrlData("tweet.fields", string.Join(",", tweetFields));
            }

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<UserV2Response>(result.Data);
            }
            else
            {
                throw new Exception(result.Data);
            }
        }

        public async Task<UserV2Response> GetUserByIdV2(string accessToken, string id, HashSet<string> userFields = null, HashSet<string> tweetFields = null, HashSet<string> expansions = null)
        {
            if (userFields == null)
            {
                userFields = UserResponseFields.User.ALL;
            }
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            _httpClient.SetApiInfo($"{ConstRouter.GetUserByIdV2}{id}", HttpMethod.Get);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, accessToken);
            apiParameter.AddToFormUrlData("user.fields", string.Join(",", userFields));
            if (tweetFields != null && tweetFields.Any())
            {
                apiParameter.AddToFormUrlData("tweet.fields", string.Join(",", tweetFields));
            }
            if (expansions != null && expansions.Any())
            {
                apiParameter.AddToFormUrlData("tweet.fields", string.Join(",", tweetFields));
            }

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<UserV2Response>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<TwitterResponse<TextCreateResponseData>> CreateTextV2(string accessToken, string text)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(text, "text");
            _httpClient.SetApiInfo(ConstRouter.TweetsV2, HttpMethod.Post);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, accessToken);
            apiParameter.SetBody(new { text });

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<TwitterResponse<TextCreateResponseData>>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<dynamic> DeleteMediaV2(string accessToken, string id)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(id, "id");
            _httpClient.SetApiInfo($"{ConstRouter.TweetsV2}/{id}", HttpMethod.Delete);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, accessToken);

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<dynamic>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<TwitterResponse<TextCreateResponseData>> CreateMediaV2(string accessToken, string text, params string[] mediaIds)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(text, "text");
            ArgumentCheck.Begin().NotNull(mediaIds, "mediaIds");
            ArgumentCheck.Begin().IsLessOrEqual(text.Length, "text", 1000);
            ArgumentCheck.Begin().IsGreaterThan(mediaIds.Length, "mediaIds", 0);
            _httpClient.SetApiInfo($"{ConstRouter.TweetsV2}", HttpMethod.Post);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, accessToken);

            apiParameter.SetBody(new
            {
                text = text,
                media = new
                {
                    media_ids = mediaIds
                }
            });
            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<TwitterResponse<TextCreateResponseData>>(result.Data);
            }
            else
            {
                return null;
            }


        }

        private void AddAuthorizationV1(ApiParameter apiParameter, string accessToken)
        {
            apiParameter.AddToHeader("Authorization", accessToken);
        }

        public async Task<string> GetRequestToken()
        {
            _httpClient.SetApiInfo(ConstRouter.RequestToken, HttpMethod.Post);
            var apiParameter = new ApiParameter();
            var url = "https://api.twitter.com/oauth/request_token";
            var oAuthHeader = OAuthHelper.GenerateOAuthHeader("POST", url, _twitterApiOptions.ApiKey, _twitterApiOptions.ApiKeySecret, additionalParams: new Dictionary<string, string> { { "oauth_callback", _twitterApiOptions.CallbackUrl } });

            AddAuthorizationV1(apiParameter, oAuthHeader);
            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                var queryParams = HttpUtility.ParseQueryString(result.Data);
                return queryParams["oauth_token"];
            }
            else
            {
                return null;
            }
        }

        public async Task<(string, string)> ConnectV1()
        {
            var requestToken = await GetRequestToken();
            Console.WriteLine("TwitterRequestToken :" + requestToken);

            var authorizationUrl = $"https://api.twitter.com/oauth/authorize?oauth_token={requestToken}";
            Console.WriteLine("TwitterAuthorizationURL :" + authorizationUrl);
            return (authorizationUrl, requestToken);
        }

        public async Task<AccessTokenResponseV1> GetAccessToken(string requestToken, string oauthVerifier)
        {
            var url = "https://api.twitter.com/oauth/access_token";
            var oAuthHeader = OAuthHelper.GenerateOAuthHeader("POST", url, _twitterApiOptions.ApiKey, _twitterApiOptions.ApiKeySecret, requestToken, _twitterApiOptions.AccessTokenSecret, additionalParams: new Dictionary<string, string> { { "oauth_verifier", oauthVerifier } });
            _httpClient.SetApiInfo("/oauth/access_token", HttpMethod.Post);
            var apiParameter = new ApiParameter();
            AddAuthorizationV1(apiParameter, oAuthHeader);
            apiParameter.AddToUrl("oauth_token", requestToken);
            apiParameter.AddToUrl("oauth_verifier", oauthVerifier);
            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                var queryParams = HttpUtility.ParseQueryString(result.Data);
                return new AccessTokenResponseV1()
                {
                    oauth_token = queryParams["oauth_token"],
                    oauth_token_secret = queryParams["oauth_token_secret"],
                    user_id = queryParams["user_id"],
                    screen_name = queryParams["screen_name"],
                };
            }
            else
            {
                return null;
            }
        }
    }
}
