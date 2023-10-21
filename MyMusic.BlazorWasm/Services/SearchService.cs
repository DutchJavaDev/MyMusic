using MyMusic.BlazorWasm.Models.Search;
using Newtonsoft.Json;
using System.Net;

namespace MyMusic.BlazorWasm.Services
{
    public sealed class SearchService
    {
        public string? Response { get; set; } = string.Empty;
        public SearchResult? Root { get; set; }
        
        public async Task Search2(string query)
        {
            var client = new HttpClient();
            // Parse exact 
            // Send to backend
            Response = await (await client.GetAsync(string.Concat("https://www.googleapis.com/youtube/v3/search?key=", "", "&","q=",query))).Content.ReadAsStringAsync();
            Root = JsonConvert.DeserializeObject<SearchResult>(Response);
        }
    }
}
