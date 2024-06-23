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
      builder.Services.AddSingleton(_ => new SQLiteAsyncConnection(Constants.DatabasePath, Constants.DatabaseFlags));
      builder.Services.AddSingleton<LocalDatabase>();
			builder.Services.AddSingleton<LogReaderService>();
			builder.Services.AddSingleton<LogWriterService>();
			builder.Services.AddSingleton<ConfigurationReaderService>();
			builder.Services.AddSingleton<ConfigurationWriterService>();
			builder.Services.AddSingleton<ArtistReaderService>();
			builder.Services.AddSingleton<ArtistWriterService>();
			builder.Services.AddSingleton<SongWriterService>();
			builder.Services.AddSingleton<SongStatusWriterService>();
			builder.Services.AddSingleton<SongStatusReaderService>();
			builder.Services.AddSingleton<SongReaderService>();
      builder.Services.AddSingleton<YoutubeSearchService>();

      builder.Services.AddScoped<ApiService>();
    }
  }
}