using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using MyMusic.Api.Services;
using System.Data;

namespace MyMusic.Api.Middleware
{
    public class PasswordAuthorization : IAuthorizationMiddlewareResultHandler
    {
        private static readonly string ServerKey = "SERVER_AUTHENTICATION";
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
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
                    .Query<string>("select server_password from mymusic.server_configuration")
                    .FirstOrDefault();
            }
            _dbLogger = dbLogger;
        }

        public async Task HandleAsync(
            RequestDelegate next, 
            HttpContext context, 
            AuthorizationPolicy policy, 
            PolicyAuthorizationResult authorizeResult)
        {
            if (!HasPassword(context) || !VerifyPassword(context, ServerPasswordHash ?? string.Empty))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await _dbLogger.LogAsync(new Exception("UnAuthorized request"));
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

        private static bool VerifyPassword(HttpContext context,string serverPasswordHash)
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
