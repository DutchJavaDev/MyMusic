using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Services
{
  public sealed class ConfigurationService(SQLiteAsyncConnection connection)
  {
    private readonly SQLiteAsyncConnection _connection = connection;
    private readonly string ApiPrefix = "/api/";

    public async Task<ServerConfiguration> GetServerConfigurationAsync()
    {
      return await _connection.Table<ServerConfiguration>()
              .FirstOrDefaultAsync(); ;
    }

    public Task<int> SaveServverconfiguration(ServerConfiguration configuration)
    {
      return _connection.UpdateAsync(configuration);
    }

    public async Task<string> GetBaseApiUrl()
    {
      var configuration = await GetServerConfigurationAsync();

      return $"{configuration.ServerUrl}{ApiPrefix}";
    }
  }
}