using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Services
{
    public sealed class ConfigurationService
    {
        private readonly SQLiteAsyncConnection _connection;
        public ConfigurationService(SQLiteAsyncConnection connection)
        {
            _connection = connection;
            CreateTable();
        }

        async void CreateTable()
        {
            await _connection.CreateTableAsync(typeof(ServerConfiguration));

            var count = await _connection.Table<ServerConfiguration>().CountAsync();

            if (count == 0)
            {
                await _connection.InsertOrReplaceAsync(new ServerConfiguration
                {
                    ApiKey = string.Empty,
                    ServerPassword = string.Empty,
                    ServerUrl = string.Empty,
                });
            }
        }

        public Task<ServerConfiguration> GetServerConfigurationAsync()
        {
            return _connection.Table<ServerConfiguration>().FirstOrDefaultAsync();
        }

        public Task<int> SaveServverconfiguration(ServerConfiguration configuration)
        {
            return _connection.UpdateAsync(configuration);
        }
    }
}
