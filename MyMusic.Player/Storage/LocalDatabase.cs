using MyMusic.Player.Blazor.Models.Logging;
using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Storage
{
  public sealed class LocalDatabase(SQLiteAsyncConnection connection)
  {
    private readonly string ApiPrefix = "/api/";

    private static readonly Type[] DatabaseSchemaTypes =
    [
        typeof(ServerConfiguration),
        typeof(LogEntry),
        typeof(MusicReference)
    ];

    public async Task InsertAsync(object reference)
    {
      // fireforget
      _ = await connection.InsertAsync(reference).ConfigureAwait(false);
    }

    public async Task<MusicReference> GetMusicReferenceByIdAsync(Guid trackingId)
    {
      return await connection.GetAsync((MusicReference mr) => mr.TrackingId == trackingId.ToString()).ConfigureAwait(false);
    }

    public async Task<IEnumerable<MusicReference>> GetAllMusicsAsync()
    {// ????????? idkn
      return await connection.QueryAsync<MusicReference>("select * from music_reference").ConfigureAwait(false);
    }

    public async Task UpdateAsync(object musicReference)
    {
      await connection.UpdateAsync(musicReference).ConfigureAwait(false);
    }

    public async Task<ServerConfiguration> GetServerConfigurationAsync()
    {
      return await connection.Table<ServerConfiguration>()
              .FirstOrDefaultAsync();
    }

    public Task<int> SaveServverconfigurationAsync(ServerConfiguration configuration)
    {
      return connection.UpdateAsync(configuration);
    }

    public async Task<string> GetBaseApiUrlAsync()
    {
      var configuration = await GetServerConfigurationAsync();

      return configuration.ServerUrl + ApiPrefix;
    }

    public async Task ClearDataAsync()
    {
      await connection.ExecuteAsync("delete from music_reference");
      await connection.ExecuteAsync("delete from logs");

#if DEBUG
      // Delete api key and the url only in debug
      await connection.ExecuteAsync("delete from serverconfiguration");
#endif
    }

    public static async Task EnsureDatebaseCreationAsync()
    {
      var connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

      // should check if some fail?
      var schemasCreationResults = await connection.CreateTablesAsync(CreateFlags.ImplicitPK, DatabaseSchemaTypes).ConfigureAwait(false);

      var count = await connection.Table<ServerConfiguration>().CountAsync().ConfigureAwait(false);

      if (count == 0)
      {
        await connection.InsertOrReplaceAsync(new ServerConfiguration
        {
          ApiKey = string.Empty,
          ServerPassword = string.Empty,
          ServerUrl = string.Empty,
        }).ConfigureAwait(false);
      }
    }
  }
}
