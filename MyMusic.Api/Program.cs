using Microsoft.AspNetCore.Authorization;
using MyMusic.Api.BackgroundServices;
using MyMusic.Api.Middleware;
using MyMusic.Api.Services;
using MyMusic.Common;
using Npgsql;
using System.Data;

// enviroment variables
var connectionString = EnviromentProvider.GetDatabaseConnectionString();

Console.WriteLine(connectionString);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(connectionString));
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
