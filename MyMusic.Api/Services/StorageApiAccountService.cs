﻿using MyMusic.Common;
using Newtonsoft.Json;

namespace MyMusic.Api.Services
{
  public class StorageApiAccountService(IHttpClientFactory http)
  {
    private readonly string? _endPoint = EnviromentProvider.GetStorageApiEndpoint();
    private readonly IHttpClientFactory _httpClientFactory = http;

    private Task<HttpResponseMessage> CreateUser(string user, string policy)
    {
      using var client = CreateClient();
      var body = JsonConvert.SerializeObject(new { User = user, Policy = policy });
      return client.PostAsync("/csu", new StringContent(body));
    }

    private HttpClient CreateClient()
    {
      var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri(_endPoint ?? "");
      return client;
    }
  }
}