using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor.Attributes;
using MyMusic.Player.Blazor.Models;
using MyMusic.Player.Blazor.Models.Logging;
using MyMusic.Player.Blazor.Models.Search;
using MyMusic.Player.Services;
using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using SQLite;
using System.Reflection;

namespace MyMusic.Player.Blazor
{
  public static class MauiAppExtension
  {
    private static readonly Type[] DatabaseSchemaTypes =
    [
        typeof(ServerConfiguration),
            typeof(LogEntry)
    ];

    public static async void EnsureDatebaseCreation(this MauiAppBuilder builder)
    {
      var connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

      // should check if some fail?
      var schemasCreationResults = await connection.CreateTablesAsync(CreateFlags.ImplicitPK, DatabaseSchemaTypes);

      var count = await connection.Table<ServerConfiguration>().CountAsync();

      if (count == 0)
      {
        await connection.InsertOrReplaceAsync(new ServerConfiguration
        {
          ApiKey = string.Empty,
          ServerPassword = string.Empty,
          ServerUrl = string.Empty,
        });
      }
    }

    public static void ConfigureMyMusicServices(this MauiAppBuilder builder)
    {
      // Local database
      builder.Services.AddTransient(sp => new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags));

      builder.Services.AddTransient<LogService>();
      builder.Services.AddTransient<ConfigurationService>();
      builder.Services.AddTransient<VideoDurationService>();
      builder.Services.AddTransient<SearchService>();
      builder.Services.AddTransient<ApiService>();
      builder.Services.AddTransient<UpdaterService>();

      // Navigation cause im lazy
      var pages = GetBlazorPages();

      var service = new NavigationElementService(pages);

      builder.Services.AddSingleton(service);
    }

    private static IEnumerable<Type> GetBlazorPages()
    {
      var types = Assembly.GetExecutingAssembly().GetTypes();
      return types.Where(i => Attribute.IsDefined(i, typeof(RouteAttribute))
      && Attribute.IsDefined(i, typeof(NavigationInfoAttribute)));
    }

    public static NavLinkElement CreateNavigationLink(this NavigationInfoAttribute navigation, RouteAttribute route)
    {
      NavLinkElement link = new(navigation.Icon, route.Template, navigation.DisplayName, navigation.Index);

      if (navigation.Index == 0)
      {
        link.IsActive = true;
      }

      return link;
    }

    public static List<SearchViewModel> ToViewModels(this List<Item> result)
    {
      if (result == null)
      {
        return Enumerable.Empty<SearchViewModel>()
            .ToList();
      }

      return result.Select(i => new SearchViewModel
      {
        Title = i.snippet.title,
        Description = i.snippet.description,
        CoverUrl = i.snippet.thumbnails.medium.url,
        VideoId = i.id.videoId
      }).ToList();
    }
  }
}