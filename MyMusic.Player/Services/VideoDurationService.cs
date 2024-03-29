﻿using Newtonsoft.Json;

namespace MyMusic.Player.Services
{
  public sealed class VideoDurationService(IHttpClientFactory _httpClientFactory,
      ConfigurationService _configurationService, LogService logService)
  {
    private readonly string VideoIdV3Url = "https://www.googleapis.com/youtube/v3/videos?part=contentDetails&";

    public async Task<Dictionary<string, TimeSpan>> GetVideoDurrations(string ids)
    {
      try
      {
        var stringContent = await GetIdsAsync(ids);

        var dynamicResult = ParseDynamic(stringContent);

        var items = ParseDynamic(dynamicResult["items"].ToString());

        return CreateDurationDictionary(items);
      }
      catch (Exception e)
      {
        await logService.Log(e, this);
        return [];
      }
    }

    private async Task<string> GetIdsAsync(string ids)
    {
      var url = await CreateUrlAsync(ids);

      var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri(url);

      var clientResult = await client.GetAsync(url);

      return await clientResult.Content.ReadAsStringAsync();
    }

    private static Dictionary<string, TimeSpan> CreateDurationDictionary(dynamic items)
    {
      var result = new Dictionary<string, TimeSpan>();

      foreach (var item in items)
      {
        var id = item["id"].ToString();

        var contentDetails = ParseDynamic(item["contentDetails"].ToString());

        result[id] = System.Xml.XmlConvert.ToTimeSpan(contentDetails["duration"].ToString());
      }

      return result;
    }

    private static dynamic ParseDynamic(string value)
    {
      return JsonConvert.DeserializeObject<dynamic>(value);
    }

    private async Task<string> CreateUrlAsync(string ids)
    {
      var apiKey = await GetApiKeyAsync();
      return string.Concat(VideoIdV3Url, apiKey, Ids(ids));
    }

    private static string Ids(string query)
    {
      return string.Concat("id=", query);
    }

    private async Task<string> GetApiKeyAsync()
    {
      var apiKey = (await _configurationService.GetServerConfigurationAsync()).ApiKey;

      if (string.IsNullOrEmpty(apiKey))
      {
        throw new Exception("Empty search key");
      }

      return string.Concat("key=", apiKey, "&");
    }
  }
}