using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor;
using MyMusic.Player.Blazor.Attributes;
using MyMusic.Player.Blazor.Models;
using System.Reflection;

namespace MyMusic.Player.Services
{
  public sealed class NavigationElementService
  {
    public IEnumerable<NavLinkElement> NavigationLinks { get; init; }

    private NavLinkElement _previousLink;

    public NavigationElementService(IEnumerable<Type> pages)
    {
      NavigationLinks = pages.Select(CombineAttributes).
                              Select(CreateNavigationLinks);
    }

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