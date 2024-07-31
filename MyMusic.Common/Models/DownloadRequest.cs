namespace MyMusic.Common.Models
{
  public sealed class DownloadRequest
  {
    public string? Name { get; set; }
    public DateTime Release { get; set; }
    public string? VideoId { get; set; }
  }

	public sealed class StatusRequest
	{
		public List<Guid> TrackingIds { get; set; }
	}
}