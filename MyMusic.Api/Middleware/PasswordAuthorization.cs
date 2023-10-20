using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Data;

namespace MyMusic.Api.Middleware
{
    public class PasswordAuthorization : IAuthorizationMiddlewareResultHandler
    {
        private static readonly string ServerKey = "SERVER_AUTHENTICATION";
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        private readonly string? ServerPasswordHash;

        public PasswordAuthorization(IDbConnection connection)
        {
            using (connection)
            {
                connection.Open();
                ServerPasswordHash = connection
                    .Query<string>("select server_password from mymusic.server_configuration")
                    .FirstOrDefault();
            }
        }

        public async Task HandleAsync(
            RequestDelegate next, 
            HttpContext context, 
            AuthorizationPolicy policy, 
            PolicyAuthorizationResult authorizeResult)
        {
            if (!HasPassword(context))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (!VerifyPassword(context, ServerPasswordHash ?? string.Empty))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // Continue
            await next(context);
            //await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }


        // Header check
        private static bool HasPassword(HttpContext context)
        {
            return context.Request.Headers.ContainsKey(ServerKey);
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
