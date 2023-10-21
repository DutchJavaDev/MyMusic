using MyMusic.BlazorWasm.Services;

namespace MyMusic.BlazorWasm.Models.Search
{
    public sealed class Item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public Id id { get; set; }
    }
}
