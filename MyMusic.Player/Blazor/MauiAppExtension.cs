﻿using MyMusic.Player.Blazor.Models.Logging;
using MyMusic.Player.Blazor.Models.Search;
using MyMusic.Player.Services;
using MyMusic.Player.Services.Youtube;
using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Blazor
{
  public static class MauiAppExtension
  {
    private static readonly Type[] DatabaseSchemaTypes =
    [
        typeof(ServerConfiguration),
        typeof(LogEntry),
        typeof(MusicReference)
    ];

    public static async Task EnsureDatebaseCreation(this MauiAppBuilder builder)
    {
      var connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

      // should check if some fail?
      var schemasCreationResults = await connection.CreateTablesAsync(CreateFlags.ImplicitPK, DatabaseSchemaTypes).ConfigureAwait(false);

      var count = await connection.Table<ServerConfiguration>().CountAsync().ConfigureAwait(false);

      if (count == 0)
      {
        await connection.InsertOrReplaceAsync(new ServerConfiguration
        {
          ApiKey = string.Empty,
          ServerPassword = string.Empty,
          ServerUrl = string.Empty,
        }).ConfigureAwait(false);
      }
    }

    public static void ConfigureMyMusicServices(this MauiAppBuilder builder)
    {
      // Local database
      builder.Services.AddTransient(_ => new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags));
      builder.Services.AddSingleton<YoutubeSearchService>();
      builder.Services.AddTransient<LocalDatabase>();
      builder.Services.AddTransient<LogService>();
      builder.Services.AddTransient<VideoDurationService>();
      builder.Services.AddTransient<ApiService>();
      builder.Services.AddTransient<UpdaterService>();
    }

    //public static List<SearchViewModel> ToViewModels(this List<Item> result)
    //{
    //  if (result == null)
    //  {
    //    return Enumerable.Empty<SearchViewModel>()
    //        .ToList();
    //  }
      
    //  return result.ConvertAll(i => new SearchViewModel
    //  {
    //    Title = i.snippet.title,
    //    Description = i.snippet.description,
    //    CoverUrl = i.snippet.thumbnails.@default.url,
    //    VideoId = i.id.videoId
    //  });
    //}
  }
}