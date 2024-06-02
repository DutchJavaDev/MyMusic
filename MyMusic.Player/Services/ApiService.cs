using MyMusic.Common.Models;
using MyMusic.Player.Services.Write;
using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace MyMusic.Player.Services
{
	public sealed class ApiService(IHttpClientFactory _httpClientFactory, 
		LocalDatabase _database,
		LogWriterService _logWriter)
	{

		public async Task<IEnumerable<MusicDto>> GetDownloadedValuesAsync()
		{
			try
			{
				var configuration = await _database.GetServerConfigurationAsync();

				var client = await ConfigureApiClient(configuration);

				var request = await client.GetAsync("music");

				var stringContent = await request.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<IEnumerable<MusicDto>>(stringContent);
			}
			catch (Exception e)
			{
				await _logWriter.Error(e, this);
				return [];
			}
		}

		public async Task<IEnumerable<StatusModel>> GetStatusModelsAsync()
		{
			try
			{
				/// "SERVER_AUTHENTICATION"
				///
				var configuration = await _database.GetServerConfigurationAsync();

				var client = await ConfigureApiClient(configuration);

				var request = await client.GetAsync("download/status");

				var stringContent = await request.Content.ReadAsStringAsync();

				if (!request.IsSuccessStatusCode)
				{
					throw new Exception(stringContent);
				}

				return JsonConvert.DeserializeObject<IEnumerable<StatusModel>>(stringContent);
			}
			catch (Exception e)
			{
				await _logWriter.Error(e, this);
				return [];
			}
		}

		public async Task<string> DownloadAsync(DownloadRequest downloadRequest)
		{
			try
			{
				// "SERVER_AUTHENTICATION"
				//
				var configuration = await _database.GetServerConfigurationAsync();

				var client = await ConfigureApiClient(configuration);

				var content = JsonConvert.SerializeObject(downloadRequest);

				// Encoding is import lol.........
				var result = await client.PostAsync("download/start", new StringContent(content, Encoding.UTF8, "application/json"));

				return await result.Content.ReadAsStringAsync();
			}
			catch (Exception e)
			{
				await _logWriter.Error(e, this);
				return string.Empty;
			}
		}

		private async Task<HttpClient> ConfigureApiClient(Configuration configuration)
		{
			if (string.IsNullOrEmpty(configuration.CloudUrl) || string.IsNullOrEmpty(configuration.CloudPassword))
			{
				throw new ArgumentException("Invalid configuration");
			}

			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(await _database.GetBaseApiUrlAsync());
			client.DefaultRequestHeaders.Authorization = new(HttpUtility.HtmlEncode(configuration.CloudPassword));
			return client;
		}
	}
}