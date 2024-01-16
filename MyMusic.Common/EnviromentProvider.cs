using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MyMusic.Common
{
    public static class EnviromentProvider
    {
        private readonly static Dictionary<string, string> _cache = [];
        
        private const string databaseStringKey = "database_url";
        private const string minioEndpointKey = "minio_endpoint";
        private const string minioUserKey = "minio_user";
        private const string minioPasswordKey = "minio_password";
        private const string minioAuthenticationApiEnpointKey = "minio_auth_api_endpoint";
       

        private static IConfigurationRoot? configurationRoot;
        private static Assembly Assembly;

        static EnviromentProvider() 
        {
            Assembly = typeof(EnviromentProvider).Assembly;
        }

        public static (string endpoint, string accessKey, string secretKey) GetMinioConfig()
        {
            return (GetValue(minioEndpointKey),
                    GetValue(minioUserKey),
                    GetValue(minioPasswordKey));
        }

        public static string GetStorageApiEndpoint()
        {
            return GetValue(minioAuthenticationApiEnpointKey);
        }

        public static string GetDatabaseConnectionString()
        {
            return GetValue(databaseStringKey);
        }

        public static void SetAssembly(Assembly assembly)
        {
            Assembly = assembly;

            // Rebuild
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddUserSecrets(Assembly);

            configurationRoot = builder.Build();
        }

        private static string GetValue(string key)
        {
            try
            {
                // Check cache
                if (_cache.TryGetValue(key, out string? value))
                {
                    return value;
                }

                // Check secrets
                if (configurationRoot is null)
                {
                    var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables()
                    .AddUserSecrets(Assembly);

                    configurationRoot = builder.Build();
                }

                if (configurationRoot[key] is not null)
                {
                    _cache[key] = configurationRoot[key] ?? string.Empty;

                    return _cache[key];
                }

            }
            catch (Exception)
            {
                throw;
            }
            // Check enviroment
            //var environment = Environment.GetEnvironmentVariables();

            //if (environment[key] is not null)
            //{
            //    _cache[key] = (string?)environment[key] ?? string.Empty;

            //    return _cache[key];
            //}

            return string.Empty;
        }
    }
}
