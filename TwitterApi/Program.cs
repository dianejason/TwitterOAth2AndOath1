using Samm.OpenApi.Adapter.Http;
using Tweetinvi;
using TwitterApiSdk;
using TwitterApiSdk.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<TwitterApiOptions>(builder.Configuration.GetSection("TwitterApiOptions"));

builder.Services.AddHttpClient();
builder.Services.AddTransient<IHttpClient, DefaultHttpClient>();
builder.Services.AddTransient<ITwitterApiClient, TwitterApiClient>();
builder.Services.AddTransient<IUploadApi, UploadApi>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void Test()
{

    // 设置Twitter API的认证信息
    var consumerKey = "YOUR_CONSUMER_KEY";
    var consumerSecret = "YOUR_CONSUMER_SECRET";
    var accessToken = "YOUR_ACCESS_TOKEN";
    var accessTokenSecret = "YOUR_ACCESS_TOKEN_SECRET";

    // 进行认证
    var userClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessTokenSecret);

    // 发布一条推文
    var tweet = userClient.Tweets.PublishTweetAsync("Hello, Twitter! This is a tweet from my C# application.").Result;

    userClient.Auth.CreateBearerTokenAsync();
}
