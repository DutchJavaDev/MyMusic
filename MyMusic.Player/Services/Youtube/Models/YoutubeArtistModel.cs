namespace MyMusic.Player.Services.Youtube.Models
{
	public class ItemCollection
	{
		public string kind { get; set; }
		public string etag { get; set; }
		public string id { get; set; }
		public List<Music> musics { get; set; }
	}

	public class Music
	{
		public string image { get; set; }
		public string videoId { get; set; }
		public string song { get; set; }
		public string artist { get; set; }
		public string album { get; set; }
	}
	public sealed class YoutubeArtistModel
	{
		public string kind { get; set; }
		public string etag { get; set; }
		public List<ItemCollection> items { get; set; }
	}
}
