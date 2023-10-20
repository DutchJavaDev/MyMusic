using MyMusic.BlazorWasm.Services;

namespace MyMusic.BlazorWasm.Models.Search
{
    public sealed class SearchResult
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string regionCode { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<Item>? items { get; set; }
    }
}
