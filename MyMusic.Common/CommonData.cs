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
            Uploading,
            Done,
            Failed
        }

        public class MusicDownload
        {
            public int? MusicId { get; set; }
            public string? Name { get; set; }
            public Mp3State? State { get; set; }
            public string? Id { get; set; }
        }
    }
}
