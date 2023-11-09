namespace MyMusic.Player.Blazor.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NavigationInfoAttribute : Attribute
    {
        public string Icon { get; init; }
        public string Href { get; init; }
        public string DisplayName { get; init; }
        public int Index { get; init; }

        public NavigationInfoAttribute(string icon, string href, string displayName, int index = int.MaxValue)
        {
            Icon = icon;
            Href = href;
            DisplayName = displayName;
            Index = index;
        }
    }
}
