1、add namespace

using Samm.OpenApi.Adapter.Http;
using TwitterApiSdk;
using TwitterApiSdk.Api;

2、add dependencies

builder.Services.AddHttpClient();
builder.Services.AddTransient<IHttpClient, DefaultHttpClient>();
builder.Services.AddTransient<ITwitterApiClient, TwitterApiClient>();
builder.Services.AddTransient<IUploadApi, UploadApi>();

3、use

sample code

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
    
            [HttpGet("GetAutheticationUrlV2")]
            public IActionResult GetAutheticationUrl()
            {
                Random random = new Random();
                var randomNum = random.Next(1, 10);
                var codeChallenge = $"sfsdf845asdfasdf1351asdf13213asfeasdf12dfa{randomNum}";// Combination of characters and numbers of length 42
                var url = twitterApi.ConnectV2(Guid.NewGuid().ToString(), codeChallenge);
                return Ok(url);
            }
    
            [HttpPost("AccessTokenV2")]
            public IActionResult AccessToken(string code, string challenge)
            {
                return Ok(twitterApi.AccessTokenV2(code, challenge));
            }
    
            [HttpPost("RefreshTokenV2")]
            public async Task<IActionResult> RefreshToken(string refreshToken)
            {
                return Ok(await twitterApi.RefreshTokenV2(refreshToken));
            }
    
            [HttpGet("GetAutheticationUrlV1")]
            public async Task<IActionResult> GetAutheticationUrlV1()
            {
                var res = await twitterApi.ConnectV1();
                return Ok(res.Item1);
            }
    
            [HttpGet("CallBack")]
            public async Task<IActionResult> CallBackGet(string oauth_token, string oauth_verifier)
            {
                Console.WriteLine($"oauth_token:{oauth_token},oauth_verifier:{oauth_verifier}");
                var result = await twitterApi.GetAccessToken(oauth_token, oauth_verifier);
                return Ok(result);
            }
    
        }
    }
    
    
    
