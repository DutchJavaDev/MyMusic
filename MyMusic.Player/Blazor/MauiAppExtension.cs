using Microsoft.AspNetCore.Components;
using System.Reflection;
using MyMusic.Player.Blazor.Attributes;
using MyMusic.Player.Blazor.Services;
using MyMusic.Player.Blazor.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SQLite;
using MyMusic.Player.Storage;

namespace MyMusic.Player.Blazor
{
    public static class MauiAppExtension
    {
        public static void ConfigureBlazorNavigation(this MauiAppBuilder builder) 
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
                return new SQLiteAsyncConnection(Constants.DatabaseFileName,Constants.Flags);
            });
        }

        private static IEnumerable<Type> GetBlazorPages()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            return types.Where(i => Attribute.IsDefined(i, typeof(RouteAttribute))
            && Attribute.IsDefined(i, typeof(NavigationInfoAttribute))).ToList();
        }

        public static NavLinkElement CreateNavigationLink(this NavigationInfoAttribute navigation)
        {
            NavLinkElement link = new(navigation.Icon, navigation.Href, navigation.DisplayName, navigation.Index);

            if (navigation.Index == 0)
            {
                link.IsActive = true;
            }

            return link;
        }
    }
}
