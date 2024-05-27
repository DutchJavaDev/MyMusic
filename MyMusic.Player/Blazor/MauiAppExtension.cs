using MyMusic.Player.Services;
using MyMusic.Player.Services.Youtube;
using MyMusic.Player.Storage;
using SQLite;

namespace MyMusic.Player.Blazor
{
  public static class MauiAppExtension
  {
    public static void ConfigureMyMusicServices(this MauiAppBuilder builder)
    {
      // Local database
      builder.Services.AddTransient(_ => new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags));
      builder.Services.AddTransient<LocalDatabase>();
      builder.Services.AddSingleton<YoutubeSearchService>();
      builder.Services.AddTransient<LogService>();
      builder.Services.AddTransient<ApiService>();
    }
  }
}