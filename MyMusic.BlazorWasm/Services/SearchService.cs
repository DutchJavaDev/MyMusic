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

        //public async Task Search(string query)
        //{
        //    var searchListRequest = _youTubeService.Search.List("snippet");
        //    searchListRequest.Q = query;
        //    searchListRequest.MaxResults = 10;

        //    // Call the search.list method to retrieve results matching the specified query term.
        //    var searchListResponse = await searchListRequest.ExecuteAsync();


        //    // Add each result to the appropriate list, and then display the lists of
        //    // matching videos, channels, and playlists.
        //    foreach (var searchResult in searchListResponse.Items)
        //    {
        //        switch (searchResult.Id.Kind)
        //        {
        //            case "youtube#video":
        //                Videos.Add(searchResult);
        //                break;

        //            case "youtube#channel":
        //                Channels.Add(searchResult);
        //                break;

        //            case "youtube#playlist":
        //                Playlists.Add(searchResult);
        //                break;
        //        }
        //    }

        //}
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Id
    {
        public string kind { get; set; }
        public string videoId { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public Id id { get; set; }
    }

    public class Root
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string regionCode { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<Item>? items { get; set; }
    }
}
