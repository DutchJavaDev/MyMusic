namespace MyMusic.Common
{
    public static class CommonData
    {
        public enum Mp3State : int
        {
            Null,
            Created,
            Downloaded,
            Converted,
            Uploaded,
            Done,
            Failed
        }

        public class MusicDownload
        {
            public int? DownloadId { get; set; }
            public string? Name { get; set; }
            public Mp3State? State { get; set; }
            public string? VideoId { get; set; }
            public string? FilePath {get; set; }
        }
    }
}
