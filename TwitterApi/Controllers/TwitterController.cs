using Microsoft.AspNetCore.Mvc;
using Samm.OpenApi.Adapter;
using TwitterApiSdk.Api;
using TwitterApiSdk.Model;

namespace TwitterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {
        private readonly ITwitterApiClient twitterApi;
        private readonly IUploadApi uploadApi;
        private readonly IHttpClientFactory httpClientFactory;
        public TwitterController(ITwitterApiClient twitterApi, IUploadApi uploadApi, IHttpClientFactory httpClientFactory)
        {
            this.twitterApi = twitterApi;
            this.uploadApi = uploadApi;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet("GetAutheticationUrl")]
        public IActionResult GetAutheticationUrl()
        {
            Random random = new Random();
            var randomNum = random.Next(1, 10);
            var codeChallenge = $"sfsdf845asdfasdf1351asdf13213asfeasdf12dfa{randomNum}";
            var url = twitterApi.ConnectV2(Guid.NewGuid().ToString(), codeChallenge);
            return Ok(url);
        }

        [HttpPost("AccessToken")]
        public IActionResult AccessToken(string code, string challenge)
        {
            return Ok(twitterApi.AccessTokenV2(code, challenge));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return Ok(await twitterApi.RefreshTokenV2(refreshToken));
        }

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser(string accessToken)
        {
            var user = await twitterApi.GetUserInfoV2(accessToken);
            return Ok(user);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetCurrentUser(string accessToken, string id)
        {
            var user = await twitterApi.GetUserByIdV2(accessToken, id);
            return Ok(user);
        }

        [HttpPost("CreateText")]
        public async Task<IActionResult> CreateText(string accessToken, string accessTokenSecret, string text)
        {
            var user = await twitterApi.CreateTextV2(accessToken, accessTokenSecret, text);
            return Ok(user);
        }

        [HttpPost("PublishMedia")]
        public async Task<IActionResult> PublishMedia(string accessToken, string accessTokenSecret, string text, string mediaId)
        {
            var publishResult = await twitterApi.CreateMediaV2(accessToken, accessTokenSecret, text, mediaId);
            return Ok(publishResult);
        }

        [HttpPost("CreateImage")]
        public async Task<IActionResult> CreateImage([FromBody] ImageUploadInfo uploadInfo)
        {
            using
            var client = httpClientFactory.CreateClient();
            var mediaList = new List<string>();

            foreach (var imageUrl in uploadInfo.Images)
            {
                var bytes = await client.GetByteArrayAsync(imageUrl);
                var type = await uploadApi.GetMimeType(imageUrl);
                var res = await uploadApi.UploadMedia(uploadInfo.AccessToken, uploadInfo.AccessTokenSecret, bytes, type, UploadMediaFileType.tweet_image);
                if (res.media_id > 0)
                    mediaList.Add(res.media_id_string);
            }

            if (mediaList.Any())
            {
                var publishResult = await twitterApi.CreateMediaV2(uploadInfo.AccessToken, uploadInfo.AccessTokenSecret, uploadInfo.Text, mediaList.ToArray());
                return Ok(publishResult);
            }
            else
            {
                return Ok("发布失败");
            }
        }


        [HttpPost("CreateVideoByPart")]
        public async Task<IActionResult> CreateVideoByPart([FromBody] VideoUploadInfo uploadInfo)
        {
            using
            var client = httpClientFactory.CreateClient();
            var bytes = await client.GetByteArrayAsync(uploadInfo.VideoUrl);
            var mimeType = await uploadApi.GetMimeType(uploadInfo.VideoUrl);
            var res = await uploadApi.InitUpload(uploadInfo.AccessToken, uploadInfo.AccessTokenSecret, bytes.Length, mimeType, UploadMediaFileType.tweet_video);
            if (res != null && !string.IsNullOrEmpty(res.media_id_string))
            {
                var partList = ArrayUtils.SplitArray2(bytes, 1 * 1024 * 1024);
                var uploadResult = new List<bool>();
                for (int index = 0; index < partList.Count; index++)
                {
                    var isSucess = await uploadApi.AppendUplaod(uploadInfo.AccessToken, uploadInfo.AccessTokenSecret, res.media_id_string, partList[index], index);
                    uploadResult.Add(isSucess);
                }
                if (uploadResult.Any(o => !o))
                {
                    return Ok("发布失败");
                }
                else
                {
                    var uploadResult2 = await uploadApi.FinalizeUplaod(uploadInfo.AccessToken, uploadInfo.AccessTokenSecret, res.media_id_string);
                    if (uploadResult2.media_id > 0)
                    {
                        if (uploadResult2.video == null)
                        {
                            await Task.Delay(uploadResult2.processing_info.check_after_secs);
                        }

                        var uploadStatus = await uploadApi.UploadStatus(uploadInfo.AccessToken, uploadInfo.AccessTokenSecret, res.media_id_string);
                        if (uploadStatus.processing_info.state == UploadProgress.Succeeded)
                        {
                            var publishResult = await twitterApi.CreateMediaV2(uploadInfo.AccessToken, uploadInfo.AccessTokenSecret, uploadInfo.Text, res.media_id_string);
                            return Ok(publishResult);
                        }
                        else if (uploadStatus.processing_info.state == UploadProgress.Failed)
                        {
                            return Ok(uploadStatus.processing_info.error?.message);
                        }
                        else if (uploadStatus.processing_info.state == UploadProgress.InProgress)
                        {
                            await Task.Delay(uploadResult2.processing_info.check_after_secs);
                        }
                        else
                        {
                            return Ok("发布失败");
                        }
                    }
                    else
                    {
                        return Ok("发布失败");
                    }
                }

                return Ok("发布失败");
            }
            else
            {
                return Ok("发布失败");
            }
        }

        [HttpGet("GetAutheticationUrlV1")]
        public async Task<IActionResult> GetAutheticationUrlV1()
        {
            var res = await twitterApi.ConnectV1();
            return Ok(res);
        }

        [HttpPost("CallBack")]
        public async Task<IActionResult> CallBack(string oauth_token, string oauth_verifier)
        {
            Console.WriteLine($"oauth_token:{oauth_token},oauth_verifier:{oauth_verifier}");
            var result = await twitterApi.GetAccessToken(oauth_token, oauth_verifier);
            return Ok(result);
        }
        [HttpGet("CallBack")]
        public async Task<IActionResult> CallBackGet(string oauth_token, string oauth_verifier)
        {
            Console.WriteLine($"oauth_token:{oauth_token},oauth_verifier:{oauth_verifier}");
            var result = await twitterApi.GetAccessToken(oauth_token, oauth_verifier);
            return Ok(result);
        }

        [HttpGet("GetUserInfoV1")]
        public async Task<IActionResult> GetUserInfoV1(string accessToken, string accessTokenSecret, string userId)
        {
            var res = await twitterApi.GetUserInfoV1(accessToken, accessTokenSecret, userId);
            return Ok(res);
        }

        [HttpGet("GetUserInfoV2")]
        public async Task<IActionResult> GetUserInfoV2(string accessToken, string accessTokenSecret)
        {
            var res = await twitterApi.GetUserInfoV2(accessToken, accessTokenSecret);
            return Ok(res);
        }

        [HttpGet("GetTimeLineV1")]
        public async Task<IActionResult> GetTimeLineV1(string accessToken, string accessTokenSecret, string userId)
        {
            var res = await twitterApi.TimeLineV1(accessToken, accessTokenSecret, userId, "10");
            return Ok(res);
        }

    }
    public class VideoUploadInfo
    {
        public string VideoUrl { get; set; }
        public string Text { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }

    public class ImageUploadInfo
    {
        public string[] Images { get; set; }
        public string Text { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}
