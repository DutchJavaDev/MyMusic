using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;


namespace MyMusic.Player.Services.Read
{
	public sealed class ConfigurationReaderService(LocalDatabase localDatabase)
	{
		public async Task<IList<Configuration>> GetConfigurationsAsync()
		{
			var column = nameof(Configuration.IsSelected);
			var query = $"select * from Configuration order by {column} desc";
			return await localDatabase.QueryAsync<Configuration>(query);
		}

		public async Task<Configuration> GetSelectedConfigurationAsync()
		{
			var column = nameof(Configuration.IsSelected);
			var query = $"select * from Configuration where {column}=?";
			var result = (await localDatabase.QueryAsync<Configuration>(query, true))
				.FirstOrDefault() ?? new();

			if(result is null)
			{
				// No current config
				throw new ArgumentNullException("No selected config, this might happen");
			}

			return result;
		}
	}
}
