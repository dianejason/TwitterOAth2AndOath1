using Samm.OpenApi.Adapter.Http;
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
