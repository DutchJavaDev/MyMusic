using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using MyMusic.Api.BackgroundServices;
using MyMusic.Api.Middleware;
using MyMusic.Api.Services;
using MyMusic.Common;
using MyMusic.Data;
using Npgsql;
using System.Data;

// enviroment variables
var connectionString = EnviromentProvider.GetDatabaseConnectionString();
var mongoDbConnectionString = EnviromentProvider.GetStorageDbConnectinString();
DatabaseMigration.EnsureDatabaseCreation(connectionString);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();

// MongoDb
builder.Services.AddTransient(_ => new MongoClient(mongoDbConnectionString));
// Postgress
builder.Services.AddTransient<IDbConnection>((_) => new NpgsqlConnection(connectionString));
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();