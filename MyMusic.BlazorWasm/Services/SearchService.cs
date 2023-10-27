using MyMusic.BlazorWasm.Models.Search;
using Newtonsoft.Json;

namespace MyMusic.BlazorWasm.Services
{
    public sealed class SearchService
    {
        private readonly string SearchV3Url = "https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=5&type=video&";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStorageService _storageService;
        public SearchService(IHttpClientFactory httpClientFactory, IStorageService storageService)
        {
            _httpClientFactory = httpClientFactory;
            _storageService = storageService;
        }

        public async Task<IEnumerable<SearcViewModel>> Search(string query)
        {
            var url = CreateUrl(query);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(url);
            var result = await client.GetAsync(url);

            return CreateViewModels(await result.Content.ReadAsStringAsync());
        }

        private static IEnumerable<SearcViewModel> CreateViewModels(string response)
        {
            var results = JsonConvert.DeserializeObject<SearchResult>(response);

            var items = results?.items;

            return items.ToViewModels();
        }

        private string CreateUrl(string query)
        {
            return string.Concat(SearchV3Url,ApiKey(), Query(query));
        }

        private string Query(string query)
        {
            return string.Concat("q=",query);
        }

        private string ApiKey()
        {
            return string.Concat("key=", _storageService.GetYouTubeDataApiKey(), "&");
        }
    }
}
