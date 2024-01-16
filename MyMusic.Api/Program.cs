using Microsoft.AspNetCore.Authorization;
using Minio;
using Minio.DataModel.Args;
using MyMusic.Api.BackgroundServices;
using MyMusic.Api.Middleware;
using MyMusic.Api.Services;
using MyMusic.Common;
using MyMusic.Data;
using Npgsql;
using System.Data;

// enviroment variables
var connectionString = EnviromentProvider.GetDatabaseConnectionString();
var (endpoint, accessKey, secretKey) = EnviromentProvider.GetMinioConfig();

DatabaseMigration.EnsureDatabaseCreation(connectionString);
await EnsureMinioBucketCreated();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
// Minio configuration
builder.Services.AddMinio(configureClient => configureClient
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey));
// Postgress
builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(connectionString));
// Services
builder.Services.AddTransient<DbLogger>();
builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, PasswordAuthorization>();
builder.Services.AddScoped<MusicService>();
builder.Services.AddScoped<DownloadService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<StorageApiAccountService>();

builder.Services.AddCors(conf => {
    conf.AddPolicy("dev_cors", policy => {
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

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("dev_cors");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


async Task EnsureMinioBucketCreated()
{
    try
    {
        var minioClient = new MinioClient()
                              .WithEndpoint(endpoint)
                              .WithCredentials(accessKey, secretKey)
                              //.WithSSL()
                              .Build();

        var bucketExistsArgs = new BucketExistsArgs();
        bucketExistsArgs.WithBucket(UploadService.BucketName);

        if (!await minioClient.BucketExistsAsync(bucketExistsArgs))
        {
            var makeBucketArgs = new MakeBucketArgs();
            makeBucketArgs.WithBucket(UploadService.BucketName);

            await minioClient.MakeBucketAsync(makeBucketArgs);
        }
    }
    catch (Exception e)
    {
        var logger = new DbLogger(new NpgsqlConnection(connectionString));
        using (logger)
        await logger.LogAsync(e);
        Console.WriteLine(e.Message);
    }
}