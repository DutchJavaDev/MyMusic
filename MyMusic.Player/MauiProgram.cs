using Microsoft.Extensions.Logging;
using MyMusic.Player.Blazor;
using MyMusic.Player.Storage;

namespace MyMusic.Player
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();

      _ = LocalDatabase.EnsureDatebaseCreationAsync();
      
      builder
          .UseMauiApp<App>()
          .ConfigureFonts(fonts =>
          {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
          });

      builder.Services.AddHttpClient();
      builder.Services.AddMauiBlazorWebView();
      
      builder.ConfigureMyMusicServices();
#if DEBUG
      builder.Services.AddBlazorWebViewDeveloperTools();
      builder.Logging.AddDebug();
#endif
      return builder.Build();
    }
  }
}