using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Samm.OpenApi.Adapter;
using Samm.OpenApi.Adapter.Http;
using TwitterApiSdk.Model;
using TwitterApiSdk.Model.Config;
using TwitterApiSdk.Model.Respense;
using TwitterApiSdk.Model.Respense.Upload;

namespace TwitterApiSdk.Api
{
    public class UploadApi : IUploadApi
    {
        private readonly IHttpClient _httpClient;
        private readonly TwitterApiOptions _twitterApiOptions;
        private readonly IHttpClientFactory _httpClientFactory;
        public UploadApi(IHttpClient httpClient,
            IOptions<TwitterApiOptions> options,
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
            _twitterApiOptions = options.Value;
            _httpClient.SetHost(_twitterApiOptions.UploadHost);
            _httpClientFactory = httpClientFactory;
        }

        private void AddAuthorization(ApiParameter apiParameter, string accessToken)
        {
            apiParameter.AddToHeader("Authorization", accessToken);
        }

        public async Task<MediaUploadResponse> UploadMedia(string accessToken, string accessTokenSecret, byte[] byteFile, string mimeType, UploadMediaFileType mediaFileType)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(byteFile, "byteFile");
            _httpClient.SetApiInfo(ConstRouter.UploadV1, HttpMethod.Post);
            var oAuthHeader = OAuthHelper.GenerateOAuthHeader("POST", ConstRouter.UploadV1FullUrl, _twitterApiOptions.ApiKey, _twitterApiOptions.ApiKeySecret, accessToken, accessTokenSecret);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, oAuthHeader);
            apiParameter.AddToFormData("media", byteFile);
            apiParameter.AddToFormData("media_category", mediaFileType.ToString());
            apiParameter.AddToFormData("media_type", mimeType);

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<MediaUploadResponse>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetMimeType(string networkFileUrl)
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, networkFileUrl);
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.Content.Headers.ContentType != null)
                {
                    return response.Content.Headers.ContentType.MediaType;
                }
            };
            return null;
        }

        public async Task<MediaUploadResponse> InitUpload(string accessToken, string accessTokenSecret, long byteSize, string mimeType, UploadMediaFileType mediaFileType)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(accessTokenSecret, "accessTokenSecret");
            ArgumentCheck.Begin().IsGreaterThan(byteSize, "byteSize", 0);

            var oAuthHeader = OAuthHelper.GenerateOAuthHeader("POST", ConstRouter.UploadV1FullUrl, _twitterApiOptions.ApiKey, _twitterApiOptions.ApiKeySecret, accessToken, accessTokenSecret);
            var apiParameter = new ApiParameter();
            _httpClient.SetApiInfo(ConstRouter.UploadV1, HttpMethod.Post);
            apiParameter.AddToHeader("Authorization", oAuthHeader);
            apiParameter.AddToFormData("command", "INIT");
            apiParameter.AddToFormData("total_bytes", byteSize);
            apiParameter.AddToFormData("media_type", mimeType);
            apiParameter.AddToFormData("media_category", mediaFileType.ToString());

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<MediaUploadResponse>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> AppendUplaod(string accessToken, string accessTokenSecret, string mediaId, byte[] part, int index)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(mediaId, "media_id");
            _httpClient.SetApiInfo(ConstRouter.UploadV1, HttpMethod.Post);

            var oAuthHeader = OAuthHelper.GenerateOAuthHeader("POST", ConstRouter.UploadV1FullUrl, _twitterApiOptions.ApiKey, _twitterApiOptions.ApiKeySecret, accessToken, accessTokenSecret);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, oAuthHeader);
            apiParameter.AddToFormData("command", "APPEND");
            apiParameter.AddToFormData("media_id", mediaId);
            apiParameter.AddToFormData("media", part);
            apiParameter.AddToFormData("segment_index", index.ToString());

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UploadStatusResponse> UploadStatus(string accessToken, string accessTokenSecret, string mediaId)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(mediaId, "mediaId");
            _httpClient.SetApiInfo(ConstRouter.UploadV1, HttpMethod.Get);
            var queryParams = new Dictionary<string, string>
        {
            { "command","STATUS" },
            { "media_id",mediaId },
        };
            var oAuthHeader = OAuthHelper.GenerateOAuthHeader("GET", ConstRouter.UploadV1FullUrl, _twitterApiOptions.ApiKey, _twitterApiOptions.ApiKeySecret, accessToken, accessTokenSecret, queryParams);
            var apiParameter = new ApiParameter();
            AddAuthorization(apiParameter, oAuthHeader);
            apiParameter.AddToUrl("command", "STATUS");
            apiParameter.AddToUrl("media_id", mediaId);

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<UploadStatusResponse>(result.Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<UploadFinalizeResponse> FinalizeUplaod(string accessToken, string accessTokenSecret, string mediaId)
        {
            ArgumentCheck.Begin().NotNull(accessToken, "accessToken");
            ArgumentCheck.Begin().NotNull(mediaId, "mediaId");
            _httpClient.SetApiInfo(ConstRouter.UploadV1, HttpMethod.Post);
            var apiParameter = new ApiParameter();
            var oAuthHeader = OAuthHelper.GenerateOAuthHeader("POST", ConstRouter.UploadV1FullUrl, _twitterApiOptions.ApiKey, _twitterApiOptions.ApiKeySecret, accessToken, accessTokenSecret);

            AddAuthorization(apiParameter, oAuthHeader);
            apiParameter.AddToFormData("command", "FINALIZE");
            apiParameter.AddToFormData("media_id", mediaId);

            var result = await _httpClient.SendAsync(apiParameter);
            if (result.IsSuccess)
            {
                return JsonExpands.ToObj<UploadFinalizeResponse>(result.Data);
            }
            else
            {
                return null;
            }
        }
    }
}
