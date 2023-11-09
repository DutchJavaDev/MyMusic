using MyMusic.Player.Blazor.Attributes;
using MyMusic.Player.Blazor.Models;

namespace MyMusic.Player.Blazor.Services
{
    public sealed class NavigationElementService
    {
        public IEnumerable<NavLinkElement> NavigationLinks { get; init; }

        public NavigationElementService(IEnumerable<Type> pages) 
        {
            var navigations = pages.Select(p => Attribute.GetCustomAttribute(p, typeof(NavigationInfoAttribute), true));

            NavigationLinks = navigations.Select(i => ((NavigationInfoAttribute)i).CreateNavigationLink());
        }

        public void SetActive(NavLinkElement element)
        {
            foreach (var link in NavigationLinks)
            {
                link.IsActive = false;

                if(link.Href == element.Href)
                {
                    link.IsActive = true;
                }
            }
        }
    }
}
