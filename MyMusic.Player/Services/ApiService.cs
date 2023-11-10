using ABI.System;
using Newtonsoft.Json;
using System.Text;
using MyMusic.Common.Models;

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

        public async Task<int> DownloadAsync(DownloadRequest downloadRequest)
        {
            try
            {
                /// "SERVER_AUTHENTICATION"
                /// 
                var configuration = await _configurationService.GetServerConfigurationAsync();
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new System.Uri(configuration.ServerUrl);
                client.DefaultRequestHeaders.Add("SERVER_AUTHENTICATION", configuration.ServerPassword);

                var content = JsonConvert.SerializeObject(downloadRequest);

                // Encoding is import lol.........
                var result = await client.PostAsync($"pipeline/downloadrequest", new StringContent(content, Encoding.UTF8, "application/json"));

                var stringContent = await result.Content.ReadAsStringAsync();

                return int.Parse(stringContent);
            }
            catch
            {
                return -1;
            }
        }
    }
}
