namespace MyMusic.Common.Models
{
  public sealed class StatusModel
  {
    public string? Name { get; set; }
    public int? State { get; set; } = 0;
    public string? Path { get; set; }
  }
}