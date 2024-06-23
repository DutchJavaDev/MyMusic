using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MyMusic.Common
{
  public static class EnviromentProvider
  {

    // If time left redo this class
    // move all keys to the right apps that need them
    // then have a single function that takes the key as argument
    // -to rectrieve the value :)
    private static readonly Dictionary<string, string> _cache = [];

    private const string databaseStringKey = "database_url";
    private const string minioEndpointKey = "minio_endpoint";
    private const string minioUserKey = "minio_user";
    private const string minioPasswordKey = "minio_password";
    private const string storageDbKey = "storage_db";
    private const string ffmpegPathKey = "ffmpeg_path";
    private const string apiKey = "data_api_key";
    private const string urlKey = "server_url";

    // Dep
    private const string minioAuthenticationApiEnpointKey = "minio_auth_api_endpoint";

    private static IConfigurationRoot? configurationRoot;
    private static Assembly Assembly;

    static EnviromentProvider()
    {
      Assembly = typeof(EnviromentProvider).Assembly;
    }

#if DEBUG
		public static string GetApiPassword()
		{
			return GetValue("API_PASSWORD");
		}

		public static string GetApiUrl()
		{
			return GetValue("API_URL");
		}
#endif

		public static string GetUrl()
    {
      return GetValue(urlKey);
    }

    public static string GetApiKey()
    {
      return GetValue(apiKey);
    }

    public static string GetStorageDbConnectinString()
    {
      return GetValue(storageDbKey);
    }

    public static string GetFfmpegPath()
    {
      return GetValue(ffmpegPathKey);
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

      return string.Empty;
    }
  }
}