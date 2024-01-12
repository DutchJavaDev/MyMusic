using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MyMusic.Common
{
    public static class EnviromentProvider
    {
        private readonly static Dictionary<string, string> _cache = new();

        private static readonly string databaseStringKey = "MM_databaseConnection";
        private static readonly string dataApiKey = "MM_dataApiKey";
        private static readonly string storageApiEnpointKey = "storageapi_endpoint";

        private static IConfigurationRoot configurationRoot;
        private static Assembly Assembly;

        static EnviromentProvider() 
        {
            Assembly = typeof(EnviromentProvider).Assembly;
        }

        public static string? GetStorageApiEndpoint()
        {
            return GetValue(storageApiEnpointKey);
        }

        public static string? GetDatabaseConnectionString()
        {
            return GetValue(databaseStringKey);
        }

        public static string? GetDataApiKey()
        {
            return GetValue(dataApiKey);
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
            // Check cache
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            // Check secrets
            if (configurationRoot is null)
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets(Assembly);

                configurationRoot = builder.Build();
            }

            if (configurationRoot[key] is not null)
            {
                _cache[key] = configurationRoot[key] ?? string.Empty;

                return _cache[key];
            }

            // Check enviroment
            var environment = Environment.GetEnvironmentVariables(
                EnvironmentVariableTarget.Process | 
                EnvironmentVariableTarget.User | 
                EnvironmentVariableTarget.Machine);

            if (environment[key] is not null)
            {
                _cache[key] = (string?)environment[key] ?? string.Empty;

                return _cache[key];
            }

            return string.Empty;
        }
    }
}
