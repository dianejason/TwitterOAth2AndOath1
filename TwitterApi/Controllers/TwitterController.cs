using Microsoft.AspNetCore.Mvc;
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
            var user = await twitterApi.GetCurrentUserV2(accessToken);
            return Ok(user);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetCurrentUser(string accessToken, string id)
        {
            var user = await twitterApi.GetUserByIdV2(accessToken, id);
            return Ok(user);
        }

        [HttpPost("CreateText")]
        public async Task<IActionResult> CreateText(string accessToken, string text)
        {
            var user = await twitterApi.CreateTextV2(accessToken, text);
            return Ok(user);
        }

        [HttpPost("CreateVideo")]
        public async Task<IActionResult> CreateVideo([FromBody] VideoUploadInfo uploadInfo)
        {
            using
            var client = httpClientFactory.CreateClient();
            var bytes = await client.GetByteArrayAsync(uploadInfo.VideoUrl);

            var res = await uploadApi.UploadMedia(uploadInfo.AccessToken, bytes, UploadMediaFileType.tweet_video);
            if (res != null && !string.IsNullOrEmpty(res.media_id_string))
            {
                var publishResult = await twitterApi.CreateMediaV2(uploadInfo.AccessToken, uploadInfo.Text, res.media_id_string);
                return Ok(publishResult);
            }
            else
            {
                return Ok("发布失败");
            }
        }

        [HttpPost("CreateImage")]
        public async Task<IActionResult> CreateImage([FromBody] ImageUploadInfo uploadInfo)
        {
            using
            var client = httpClientFactory.CreateClient();
            var mediaList = new List<string>();

            foreach (var item in uploadInfo.Images)
            {
                var bytes = await client.GetByteArrayAsync(item);
                var res = await uploadApi.UploadMedia(uploadInfo.AccessToken, bytes, UploadMediaFileType.tweet_image);
                if (res.media_id > 0)
                    mediaList.Add(res.media_id_string);
            }

            if (mediaList.Any())
            {
                var publishResult = await twitterApi.CreateMediaV2(uploadInfo.AccessToken, uploadInfo.Text, mediaList.ToArray());
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
            var res = await uploadApi.InitUpload(uploadInfo.AccessToken, bytes.Length, mimeType, UploadMediaFileType.tweet_video);
            if (res != null && !string.IsNullOrEmpty(res.media_id_string))
            {
                var publishResult = await twitterApi.CreateMediaV2(uploadInfo.AccessToken, uploadInfo.Text, res.media_id_string);
                return Ok(publishResult);
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

    }
    public class VideoUploadInfo
    {
        public string VideoUrl { get; set; }
        public string Text { get; set; }
        public string AccessToken { get; set; }
    }

    public class ImageUploadInfo
    {
        public string[] Images { get; set; }
        public string Text { get; set; }
        public string AccessToken { get; set; }
    }
}
