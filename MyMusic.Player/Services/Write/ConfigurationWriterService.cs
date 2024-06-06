using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services.Write
{
	public sealed class ConfigurationWriterService(LocalDatabase localDatabase)
	{
		public async Task UpdateConfigurationAsync(Configuration configuration)
		{
			if (configuration.IsSelected)
			{
				await RemoveCurrentSelectedConfiguration();
			}
			await localDatabase.UpdateAsync(configuration).ConfigureAwait(false);
		}

		public async Task AddConfigurationAsync(Configuration configuration)
		{
			if (configuration.IsSelected)
			{
				await RemoveCurrentSelectedConfiguration();
			}
			await localDatabase.InsertAsync(configuration).ConfigureAwait(false);
		}

		public async Task DeleteConfigurationAsync(Configuration configuration)
		{
			//  Set the next configuration as the selected if any?
			await localDatabase.DeleteAsync(configuration).ConfigureAwait(false);
		}

		private async Task RemoveCurrentSelectedConfiguration()
		{
			var column = nameof(Configuration.IsSelected);
			var query = $"update configuration set {column}=? where {column}=?";
			await localDatabase.ExecuteAsync(query, false, true).ConfigureAwait(false);
		}
	}
}
