using Microsoft.Extensions.Logging;
using MyMusic.Player.Blazor;

namespace MyMusic.Player
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();
      builder
          .UseMauiApp<App>()
          .ConfigureFonts(fonts =>
          {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
          });

      _ = builder.EnsureDatebaseCreation();

      builder.ConfigureMyMusicServices();

      builder.Services.AddHttpClient();

      builder.Services.AddMauiBlazorWebView();
#if DEBUG
      builder.Services.AddBlazorWebViewDeveloperTools();
      builder.Logging.AddDebug();
#endif
      return builder.Build();
    }
  }
}