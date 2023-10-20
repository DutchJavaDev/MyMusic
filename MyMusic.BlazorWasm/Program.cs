using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyMusic.BlazorWasm;
using MyMusic.BlazorWasm.Services;
using MyMusic.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient {});
builder.Services.AddTransient(sp => new YouTubeService(new BaseClientService.Initializer()
{
    // redo this
    ApiKey = "AIzaSyBt91nyVg3hdwEFT4iUWOk8kb1oIFhbCYs",
    ApplicationName = "DexerKeyMM"
}));
builder.Services.AddSingleton<SearchService>();
await builder.Build().RunAsync();
