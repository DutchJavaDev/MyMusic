using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyMusic.BlazorWasm;
using MyMusic.BlazorWasm.Services;
using MyMusic.Common;

EnviromentProvider.SetAssembly(typeof(Program).Assembly);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IStorageService>(sp => {

    var storageInterface = sp.GetRequiredService<ISyncLocalStorageService>();
    return new DefaultStorage(storageInterface);
});
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<SearchService>();

await builder.Build().RunAsync();
