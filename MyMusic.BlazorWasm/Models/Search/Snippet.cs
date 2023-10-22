using Google.Apis.YouTube.v3.Data;

namespace MyMusic.BlazorWasm.Models.Search
{
    public sealed class Snippet
    {
        //public DateTime publishedAt { get; set; }
        //public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        //public string liveBroadcastContent { get; set; }
        //public DateTime publishTime { get; set; }
    }
}
