namespace MyMusic.Player.Blazor.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NavigationInfoAttribute : Attribute
    {
        public string Icon { get; init; }
        public string DisplayName { get; init; }
        public int Index { get; init; }

        public NavigationInfoAttribute(string icon, string displayName, int index = int.MaxValue)
        {
            Icon = icon;
            DisplayName = displayName;
            Index = index;
        }
    }
}
