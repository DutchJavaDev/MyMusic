using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Services
{
    public sealed class ConfigurationService(SQLiteAsyncConnection connection)
    {
        private readonly SQLiteAsyncConnection _connection = connection;

        private static ServerConfiguration _cachedConfig = null;

        public async Task<ServerConfiguration> GetServerConfigurationAsync()
        {
            _cachedConfig ??= await _connection.Table<ServerConfiguration>().FirstOrDefaultAsync();

            return _cachedConfig;
        }

        public Task<int> SaveServverconfiguration(ServerConfiguration configuration)
        {
            _cachedConfig = configuration;
            return _connection.UpdateAsync(configuration);
        }
    }
}
