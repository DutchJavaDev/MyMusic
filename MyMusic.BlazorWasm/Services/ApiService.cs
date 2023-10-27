using MyMusic.Common.Models;
using Newtonsoft.Json;
using System.Text;

namespace MyMusic.BlazorWasm.Services
{
    public sealed class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string Url;
        private readonly string Password;
        public ApiService(IHttpClientFactory httpClientFactory, IStorageService storageService)
        {
            _httpClientFactory = httpClientFactory;
            Url = storageService.GetMyMusicServerURL();
            Password = storageService.GetServerPassword();
        }

        public async Task<Guid> DownloadAsync(SongDownloadRequest songDownloadRequest)
        {
            /// "SERVER_AUTHENTICATION"
            /// 
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(Url);
            client.DefaultRequestHeaders.Add("SERVER_AUTHENTICATION", Password);
            
            var content = JsonConvert.SerializeObject(songDownloadRequest);
            
            var result = await client.PostAsync($"pipeline/downloadrequest",new StringContent(content, Encoding.UTF8, "application/json"));

            var stringContent = await result.Content.ReadAsStringAsync();
            
            return new Guid(stringContent);
        }
    }
}
