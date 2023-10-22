using MyMusic.BlazorWasm.Models.Search;
using Newtonsoft.Json;

namespace MyMusic.BlazorWasm.Services
{
    public sealed class SearchService
    {
        private readonly string SearchV3Url = "https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=5&type=video&";

        private readonly HttpClient _client;
        private readonly IStorageService _storageService;
        public SearchService(HttpClient client, IStorageService storageService)
        {
            _client = client;
            _storageService = storageService;
        }

        public async Task<IEnumerable<SearcViewModel>> Search(string query)
        {
            var url = CreateUrl(query);

            var result = await _client.GetAsync(url);

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
