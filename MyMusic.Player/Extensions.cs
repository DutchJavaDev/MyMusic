namespace MyMusic.Player
{
	public static class Extensions
	{
		public static string ConvertIntToStringTime(this int duration)
		{
			var timeSpan = TimeSpan.FromSeconds(duration);

			return timeSpan.ToString(@"mm\:ss");
		}
	}
}
