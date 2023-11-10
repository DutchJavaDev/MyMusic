namespace MyMusic.Common.Models
{
    public sealed class DownloadRequest
    {
        public string Name { get; set; }
        public DateTime Release { get; set; }
        public string DownloadId { get; set; }
    }
}
