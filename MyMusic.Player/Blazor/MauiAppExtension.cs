using Microsoft.AspNetCore.Components;
using System.Reflection;
using MyMusic.Player.Blazor.Attributes;
using MyMusic.Player.Blazor.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SQLite;
using MyMusic.Player.Storage;
using MyMusic.Player.Blazor.Models.Search;
using MyMusic.Player.Services;

namespace MyMusic.Player.Blazor
{
    public static class MauiAppExtension
    {
        public static void ConfigureMyMusicNavigation(this MauiAppBuilder builder) 
        {
            var pages = GetBlazorPages();

            var service = new NavigationElementService(pages);

            builder.Services.AddSingleton(service);
        }

        public static void ConfigureMyMusicServices(this MauiAppBuilder builder)
        {
            // Local database
            builder.Services.TryAddTransient(sp =>
            {
                return new SQLiteAsyncConnection(Constants.DatabasePath,Constants.Flags);
            });

            builder.Services.AddTransient<ConfigurationService>();
            builder.Services.AddTransient<VideoDurationService>();
            builder.Services.AddTransient<SearchService>();
            builder.Services.AddTransient<ApiService>();
        }

        private static IEnumerable<Type> GetBlazorPages()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            return types.Where(i => Attribute.IsDefined(i, typeof(RouteAttribute))
            && Attribute.IsDefined(i, typeof(NavigationInfoAttribute))).ToList();
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
