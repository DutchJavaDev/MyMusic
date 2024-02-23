namespace MyMusic.Player.Blazor.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class NavigationInfoAttribute(string icon, string displayName, int index = 100) : Attribute
  {
    public string Icon { get; init; } = icon;
    public string DisplayName { get; init; } = displayName;
    public int Index { get; init; } = index;
  }
}