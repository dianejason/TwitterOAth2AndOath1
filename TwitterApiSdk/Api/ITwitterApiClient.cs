using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterApiSdk.Model.Respense;

namespace TwitterApiSdk.Api
{
    public interface ITwitterApiClient
    {
        string ConnectV2(string state, string codeChallenge);
        Task<AccessTokenResponse> AccessTokenV2(string code, string challenge);
        Task<AccessTokenResponse> RefreshTokenV2(string refreshToken);
        Task<dynamic> RevokeV2(string accessToken);
        Task<UserV2Response> GetUserInfoV2(string accessToken, HashSet<string> userFields = null, HashSet<string> tweetFields = null, HashSet<string> expansions = null);
        Task<UserV2Response> GetUserByIdV2(string accessToken, string id, HashSet<string> userFields = null, HashSet<string> tweetFields = null, HashSet<string> expansions = null);

        Task<TwitterResponse<TextCreateResponseData>> CreateTextV2(string accessToken, string accessTokenSecret, string text);
        Task<dynamic> DeleteMediaV2(string accessToken, string accessTokenSecret, string id);
        Task<TwitterResponse<TextCreateResponseData>> CreateMediaV2(string accessToken, string accessTokenSecret, string text, params string[] mediaIds);
        Task<string> GetRequestToken();
        Task<(string, string)> ConnectV1();
        Task<AccessTokenResponseV1> GetAccessToken(string requestToken, string oauthVerifier);
        Task<dynamic> GetUserInfoV1(string accessToken, string accessTokenSecret, params string[] userIds);
        Task<UserV2Response> GetUserInfoV2(string accessToken, string accessTokenSecret, HashSet<string> userFields = null, HashSet<string> tweetFields = null, HashSet<string> expansions = null);
        Task<dynamic> TimeLineV2(string accessToken, string userId, string maxResults, string paginationToken = null, string startTime = null, string endTime = null, string sinceId = null, string untilId = null, HashSet<string> expansions = null, HashSet<string> tweetFields = null, HashSet<string> userFields = null, HashSet<string> mediaFields = null, HashSet<string> placeFields = null, HashSet<string> pollFields = null);
        Task<dynamic> TimeLineV1(string accessToken, string accessTokenSecret, string userId, string count, string includeRts = "0", string excludeReplies = "1");
    }
}
