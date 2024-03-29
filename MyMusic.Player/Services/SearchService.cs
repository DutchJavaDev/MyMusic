﻿using MyMusic.Player.Blazor;
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

    public async Task<List<SearchViewModel>> SearchAsync(string query)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(query))
        {
          return Enumerable.Empty<SearchViewModel>().ToList();
        }

        var url = await CreateUrlAsync(query).ConfigureAwait(false);

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(url);
        var result = await client.GetAsync(url).ConfigureAwait(false);

        var response = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

        return await CreateViewModels(response);
      }
      catch (Exception e)
      {
        await _logService.Log(e, this).ConfigureAwait(false);

        // js notification service popup?
        return Enumerable.Empty<SearchViewModel>().ToList();
      }
    }

    private async Task<List<SearchViewModel>> CreateViewModels(string response)
    {
      var results = JsonConvert.DeserializeObject<SearchResult>(response);

      var models = results?.items.ToViewModels();

      // Get all ids
      var ids = CreateIdString(models);

      // Get durations
      // WHY WHY WHY youtube whyyy do i have to make a second request just to get the durations.......
      var videoIds = await _videoDurationService.GetVideoDurrations(ids).ConfigureAwait(false);

      // Update
      foreach (var model in models)
      {
        model.Durration = videoIds[model.VideoId];
      }

      return models;
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
      var apiKey = await GetApiKeyAsync().ConfigureAwait(false);
      return string.Concat(SearchV3Url, apiKey, Query(query));
    }

    private string Query(string query)
    {
      return string.Concat("q=", query);
    }

    private async Task<string> GetApiKeyAsync()
    {
      var configuration = await _configurationService.GetServerConfigurationAsync().ConfigureAwait(false);

      return string.Concat("key=", configuration.ApiKey, "&");
    }
  }
}