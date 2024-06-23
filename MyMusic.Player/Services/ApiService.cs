using MyMusic.Common.Models;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Services.Write;
using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Web;

namespace MyMusic.Player.Services
{
	public sealed class ApiService(IHttpClientFactory _httpClientFactory, 
		ConfigurationReaderService _configurationReader,
		LogWriterService _logWriter)
	{

		public async Task<IEnumerable<StatusModel>> GetStatusModelsAsync(StatusRequest statusRequest)
		{
			try
			{
				/// "SERVER_AUTHENTICATION"
				///
				var configuration = await _configurationReader.GetSelectedConfigurationAsync();

				var client = ConfigureApiClient(configuration);

				var content = JsonConvert.SerializeObject(statusRequest);

				var request = await client.PostAsync("api/download/status", new StringContent(content, Encoding.UTF8, "application/json"));

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
				var configuration = await _configurationReader.GetSelectedConfigurationAsync();

				var client = ConfigureApiClient(configuration);

				var content = JsonConvert.SerializeObject(downloadRequest);

				// Encoding is import lol.........
				var request = await client.PostAsync("api/download/start", new StringContent(content, Encoding.UTF8, "application/json"));

				var stringContent =  await request.Content.ReadAsStringAsync();

				if (!request.IsSuccessStatusCode)
				{
					throw new Exception(stringContent);
				}

				return stringContent;
			}
			catch (Exception e)
			{
				await _logWriter.Error(e, this);
				return string.Empty;
			}
		}

		private HttpClient ConfigureApiClient(Configuration configuration)
		{
			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(configuration.CloudUrl);
			client.DefaultRequestHeaders.Authorization = new(HttpUtility.HtmlEncode(configuration.CloudPassword));
			return client;
		}
	}
}