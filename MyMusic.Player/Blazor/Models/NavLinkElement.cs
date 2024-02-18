namespace MyMusic.Player.Blazor.Models
{
  public sealed class NavLinkElement
  {
    public string Icon { get; init; }
    public string Href { get; init; }
    public string DisplayName { get; init; }
    public bool IsActive { get; set; }
    public int Index { get; set; }

    public NavLinkElement(string icon, string href, string displayName, int index)
    {
      Icon = icon;
      Href = href;
      DisplayName = displayName;
      Index = index;
    }
  }
}