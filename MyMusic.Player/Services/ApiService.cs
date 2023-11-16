using Newtonsoft.Json;
using System.Text;
using MyMusic.Common.Models;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services
{
    public sealed class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationService _configurationService;

        public ApiService(IHttpClientFactory httpClientFactory, ConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<StatusModel>> GetStatusModelsAsync()
        {
            try
            {
                /// "SERVER_AUTHENTICATION"
                /// 
                var configuration = await _configurationService.GetServerConfigurationAsync();

                var client = ConfigureApiClient(configuration);

                var request = await client.GetAsync("download/status");

                var stringContent = await request.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<StatusModel>>(stringContent);
            }
            catch (Exception e)
            {
                return new StatusModel[] { new (){Name=e.Message } };
            }
        }

        public async Task<int> DownloadAsync(DownloadRequest downloadRequest)
        {
            try
            {
                /// "SERVER_AUTHENTICATION"
                /// 
                var configuration = await _configurationService.GetServerConfigurationAsync();

                var client = ConfigureApiClient(configuration);

                var content = JsonConvert.SerializeObject(downloadRequest);

                // Encoding is import lol.........
                var result = await client.PostAsync("download/start", new StringContent(content, Encoding.UTF8, "application/json"));

                var stringContent = await result.Content.ReadAsStringAsync();

                return int.Parse(stringContent);
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        private HttpClient ConfigureApiClient(ServerConfiguration configuration)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(string.Concat(configuration.ServerUrl, "/api/"));
            client.DefaultRequestHeaders.Add("SERVER_AUTHENTICATION", configuration.ServerPassword);

            return client;
        }
    }
}
