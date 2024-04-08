using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using MongoDB.Driver;
using MyMusic.Api.BackgroundServices;
using MyMusic.Api.Middleware;
using MyMusic.Api.Services;
using MyMusic.Common;
using MyMusic.Data;
using Npgsql;
using System.Data;
using System.Net;

// enviroment variables
var postgresConnectionString = EnviromentProvider.GetDatabaseConnectionString();
var mongoDbConnectionString = EnviromentProvider.GetStorageDbConnectinString();

DatabaseMigration.EnsureDatabaseCreation(postgresConnectionString);

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
  // Configure forwarded headers
  builder.Services.Configure<ForwardedHeadersOptions>(options =>
  {
    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
  });
}

// Add services to the container.
builder.Services.AddHttpClient();

// MongoDb
builder.Services.AddTransient(_ => new MongoClient(mongoDbConnectionString));
// Postgress
builder.Services.AddTransient<IDbConnection>((_) => new NpgsqlConnection(postgresConnectionString));
// Services
builder.Services.AddTransient<DbLogger>();
builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, PasswordAuthorization>();
builder.Services.AddScoped<MusicService>();
builder.Services.AddScoped<DownloadService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<MyMusicCollectionService>();

builder.Services.AddCors(conf =>
{
  conf.AddPolicy("dev_cors", policy =>
  {
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
  });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddHostedService<DownloadRequestService>();
builder.Services.AddHostedService<MongoDbUploadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseCors("dev_cors");
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseMiddleware<RequestLoggingMiddleware>();
}
else
{
  app.UseForwardedHeaders(new ForwardedHeadersOptions
  {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
  });
  app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();