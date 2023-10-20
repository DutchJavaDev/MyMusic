using Microsoft.AspNetCore.Authorization;
using MyMusic.Api.Middleware;
using MyMusic.Common;
using Npgsql;
using System.Data;

// enviroment variables
var connectionString = EnviromentProvider.GetDatabaseConnectionString();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(connectionString));

builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler, PasswordAuthorization>();

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
