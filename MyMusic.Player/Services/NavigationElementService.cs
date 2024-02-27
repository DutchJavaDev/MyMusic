using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor;
using MyMusic.Player.Blazor.Attributes;
using MyMusic.Player.Blazor.Models;
using System.Reflection;

namespace MyMusic.Player.Services
{
  public sealed class NavigationElementService(IEnumerable<Type> pages)
  {
    public IEnumerable<NavLinkElement> NavigationLinks { get; init; } = pages.Select(CombineAttributes).
                                                                              Select(CreateNavigationLinks);

    private NavLinkElement _previousLink;

    public void SetActive(NavLinkElement element)
    {
      if (_previousLink != null)
      {
        _previousLink.IsActive = false;
      }

      element.IsActive = true;

      _previousLink = element;
    }

    private static T GetCustomAttribute<T>(MemberInfo member) where T : class
    {
      return Attribute.GetCustomAttribute(member, typeof(T), true) as T;
    }

    private static (NavigationInfoAttribute navigation, RouteAttribute route) CombineAttributes(Type blazorPage)
    {
      var navigation = GetCustomAttribute<NavigationInfoAttribute>(blazorPage);
      var route = GetCustomAttribute<RouteAttribute>(blazorPage);
      return (navigation, route);
    }

    private static NavLinkElement CreateNavigationLinks((NavigationInfoAttribute, RouteAttribute) tuple)
    {
      return tuple.Item1.CreateNavigationLink(tuple.Item2);
    }
  }
}