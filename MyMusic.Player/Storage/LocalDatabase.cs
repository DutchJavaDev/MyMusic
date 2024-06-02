using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Storage
{
  public sealed class LocalDatabase(SQLiteAsyncConnection connection)
  {
    private readonly string ApiPrefix = "/api/";

    private static readonly Type[] DatabaseSchemaTypes =
    [
        typeof(Configuration),
        typeof(Log),
        typeof(Song),
			  typeof(Artists),
			  typeof(SongStatus),
    ];

    public async Task InsertAsync(object @object)
    {
      _ = await connection.InsertAsync(@object).ConfigureAwait(false);
    }

    public async Task UpdateAsync(object @object)
    {
      _ = await connection.UpdateAsync(@object).ConfigureAwait(false);
    }

		public async Task<List<T>> QueryAsync<T>(string query, object parameters = null) where T : new()
		{
			return await connection.QueryAsync<T>(query, parameters).ConfigureAwait(false);
		}

		public async Task ExecuteAsync(string query, object parameters)
		{
			_ = await connection.ExecuteAsync(query, parameters).ConfigureAwait(false);
		}

		public async Task DeleteAsync(object @object)
		{
			await connection.DeleteAsync(@object).ConfigureAwait(false);
		}

    public async Task<Configuration> GetServerConfigurationAsync()
    {
      return await connection.Table<Configuration>()
              .FirstOrDefaultAsync();
    }

    public async Task<string> GetBaseApiUrlAsync()
    {
      var configuration = await GetServerConfigurationAsync();

      return configuration.CloudUrl + ApiPrefix;
    }

    public static async Task EnsureDatebaseCreationAsync()
    {
      var connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
      _ = await connection.CreateTablesAsync(CreateFlags.ImplicitPK, DatabaseSchemaTypes).ConfigureAwait(false);
    }
  }
}
