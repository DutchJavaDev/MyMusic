﻿using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using MyMusic.Api.Services;
using System.Data;

namespace MyMusic.Api.Middleware
{
  public class PasswordAuthorization : IAuthorizationMiddlewareResultHandler
  {
    private const string ServerKey = "Authorization";
    private readonly string? ServerPasswordHash;
    private readonly DbLogger _dbLogger;

    public PasswordAuthorization(
        IDbConnection connection,
        DbLogger dbLogger)
    {
      using (connection)
      {
        connection.Open();
        ServerPasswordHash = connection
            .Query<string>("select server_password from server_configuration")
            .FirstOrDefault();
      }

      if(string.IsNullOrEmpty(ServerPasswordHash))
      {
        dbLogger.LogAsync(new Exception("Failed to get server password"), messagePrefix: "server passpwrd").Wait();
      }

      _dbLogger = dbLogger;
    }

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
      Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");

      if (!HasPassword(context) || !VerifyPassword(context, ServerPasswordHash ?? string.Empty))
      {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await _dbLogger.LogAsync(new Exception("UnAuthorized request"), messagePrefix: "PasswordAuthorization");
        return;
      }

      // Continue
      await next(context);
    }

    // Header check
    private static bool HasPassword(HttpContext context)
    {
      return context.Request.Headers.ContainsKey(ServerKey) ||
             context.Request.Query.ContainsKey(ServerKey);
    }

    private static bool VerifyPassword(HttpContext context, string serverPasswordHash)
    {
      return GetRequestPassword(context) == serverPasswordHash;
    }

    private static string? GetRequestPassword(HttpContext context)
    {
      // check headers
      var headerPassword = context.Request.Headers[ServerKey];

      if (string.IsNullOrEmpty(headerPassword))
      {
        // check query parameters
        headerPassword = context.Request.Query[ServerKey];
      }

      return headerPassword;
    }
  }
}