using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MyMusic.Common
{
    public static class EnviromentProvider
    {
        private readonly static Dictionary<string, string> _cache = new();

        private static readonly string databaseStringKey = "MM_databaseConnection";

        private static IConfigurationRoot? configurationRoot;

        public static string? GetDatabaseConnectionString()
        {
            return GetValue(databaseStringKey);
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
                .AddUserSecrets(typeof(EnviromentProvider).Assembly);

                configurationRoot = builder.Build();
            }


            if (configurationRoot[key] is not null)
            {
                _cache[key] = configurationRoot[key];

                return _cache[key];
            }

            // Check enviroment
            var environment = Environment.GetEnvironmentVariables();

            if (environment[key] is not null)
            {
                _cache[key] = (string?)environment[key] ?? string.Empty;

                return _cache[key];
            }

            return string.Empty;
        }
    }
}
