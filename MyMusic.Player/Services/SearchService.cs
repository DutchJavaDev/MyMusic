using MyMusic.Player.Blazor;
using MyMusic.Player.Blazor.Models.Logging;
using MyMusic.Player.Blazor.Models.Search;
using Newtonsoft.Json;
using System.Text;

namespace MyMusic.Player.Services
{
    public sealed class SearchService
    {
        private readonly string SearchV3Url = "https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=30&type=video&";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationService _configurationService;
        private readonly VideoDurationService _videoDurationService;
        private readonly LogService _logService;
        public static readonly List<SearchViewModel> SearchResults = Array.Empty<SearchViewModel>()
            .ToList();

        public SearchService(IHttpClientFactory httpClientFactory,
            ConfigurationService configurationService,
            VideoDurationService videoDurationService,
            LogService log)
        {
            _httpClientFactory = httpClientFactory;
            _configurationService = configurationService;
            _videoDurationService = videoDurationService;
            _logService = log;
        }

        public async Task SearchAsync(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return;
                }

                var url = await CreateUrlAsync(query);

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(url);
                var result = await client.GetAsync(url);

                CreateViewModels(await result.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                await _logService.WriteLogAsync(LogEntry.FromException(e));

                // js notification service popup?
            }
        }

        private async void CreateViewModels(string response)
        {
            var results = JsonConvert.DeserializeObject<SearchResult>(response);

            var models = results?.items.ToViewModels();

            // Get all ids
            var ids = CreateIdString(models);

            // Get durations
            // WHY WHY WHY youtube whyyy do i have to make a second request just ot get the durations.......
            var videoIds = await _videoDurationService.GetVideoDurrations(ids);

            // Update
            foreach (var model in models)
            {
                model.Durration = videoIds[model.VideoId];
            }

            SearchResults.Clear();
            SearchResults.AddRange(models);
        }

        private static string CreateIdString(IEnumerable<SearchViewModel> models)
        {
            var builder = new StringBuilder();

            foreach (var model in models)
            {
                if (models.Last() == model)
                {
                    builder.Append(model.VideoId);
                }
                else
                {
                    builder.Append(string.Concat(model.VideoId, ','));
                }
            }

            return builder.ToString();
        }

        private async Task<string> CreateUrlAsync(string query)
        {
            var apiKey = await GetApiKeyAsync();
            return string.Concat(SearchV3Url, apiKey, Query(query));
        }

        private string Query(string query)
        {
            return string.Concat("q=", query);
        }

        private async Task<string> GetApiKeyAsync()
        {
            var apiKey = (await _configurationService.GetServerConfigurationAsync()).ApiKey;
            return string.Concat("key=", apiKey, "&");
        }
    }
}
