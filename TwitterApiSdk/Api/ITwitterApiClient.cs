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
        Task<UserV2Response> GetCurrentUserV2(string accessToken, HashSet<string> userFields = null, HashSet<string> tweetFields = null, HashSet<string> expansions = null);
        Task<UserV2Response> GetUserByIdV2(string accessToken, string id, HashSet<string> userFields = null, HashSet<string> tweetFields = null, HashSet<string> expansions = null);
        Task<TwitterResponse<TextCreateResponseData>> CreateTextV2(string accessToken, string text);
        Task<dynamic> DeleteMediaV2(string accessToken, string id);
        Task<TwitterResponse<TextCreateResponseData>> CreateMediaV2(string accessToken, string text, params string[] mediaIds);

        Task<string> GetRequestToken();
        Task<(string, string)> ConnectV1();
        Task<AccessTokenResponseV1> GetAccessToken(string requestToken, string oauthVerifier);
    }
}
