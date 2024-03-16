using MyMusic.Common.Models;
using MyMusic.Player.Storage.Models;
using Newtonsoft.Json;
using System.Text;
  
namespace MyMusic.Player.Services
{
  public sealed class ApiService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ConfigurationService _configurationService;
    private readonly LogService _logService;

    public ApiService(IHttpClientFactory httpClientFactory,
        ConfigurationService configurationService,
        LogService logService)
    {
      _configurationService = configurationService;
      _httpClientFactory = httpClientFactory;
      _logService = logService;
    }

    public async Task<IEnumerable<MusicDto>> GetDownloadedValuesAsync()
    {
      try
      {
        var configuration = await _configurationService.GetServerConfigurationAsync();

        var client = await ConfigureApiClient(configuration);

        var request = await client.GetAsync("music");

        var stringContent = await request.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<MusicDto>>(stringContent);
      }
      catch (Exception e)
      {
        await _logService.Log(e, this);

        throw;
      }
    }

    public async Task<IEnumerable<StatusModel>> GetStatusModelsAsync()
    {
      try
      {
        /// "SERVER_AUTHENTICATION"
        ///
        var configuration = await _configurationService.GetServerConfigurationAsync();

        var client = await ConfigureApiClient(configuration);

        var request = await client.GetAsync("download/status");

        var stringContent = await request.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<StatusModel>>(stringContent);
      }
      catch (Exception e)
      {
        await _logService.Log(e, this);

        throw;
      }
    }

    public async Task<string> DownloadAsync(DownloadRequest downloadRequest)
    {
      try
      {
        // "SERVER_AUTHENTICATION"
        //
        var configuration = await _configurationService.GetServerConfigurationAsync();

        var client = await ConfigureApiClient(configuration);

        var content = JsonConvert.SerializeObject(downloadRequest);

        // Encoding is import lol.........
        var result = await client.PostAsync("download/start", new StringContent(content, Encoding.UTF8, "application/json"));

        return await result.Content.ReadAsStringAsync();
      }
      catch (Exception e)
      {
        await _logService.Log(e, this);

        throw;
      }
    }

    private async Task<HttpClient> ConfigureApiClient(ServerConfiguration configuration)
    {
      if (string.IsNullOrEmpty(configuration.ServerUrl) || string.IsNullOrEmpty(configuration.ServerPassword))
      {
        throw new ArgumentException("Invalid configuration");
      }

      var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri(await _configurationService.GetBaseApiUrl());
      client.DefaultRequestHeaders.Authorization = new(configuration.ServerPassword);
      return client;
    }
  }
}