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
                }).ConfigureBlazorNavigation();

            builder.ConfigureMyMusicServices();

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}