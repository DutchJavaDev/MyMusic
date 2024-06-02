using MyMusic.Player.Services;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Services.Write;
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
      builder.Services.AddSingleton(_ => new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags));
      builder.Services.AddSingleton<LocalDatabase>();
			builder.Services.AddSingleton<LogReaderService>();
			builder.Services.AddSingleton<LogWriterService>();
      builder.Services.AddSingleton<YoutubeSearchService>();

      builder.Services.AddSingleton<ApiService>();
    }
  }
}