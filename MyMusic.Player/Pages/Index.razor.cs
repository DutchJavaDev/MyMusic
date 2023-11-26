using Microsoft.AspNetCore.Components;
using MyMusic.Common.Models;
using MyMusic.Player.Blazor.Models.Search;
using MyMusic.Player.Services;

namespace MyMusic.Player.Pages
{
    public partial class Index
    {
        [Inject]
        public SearchService SearchService { get; set; }

        [Inject]
        public ApiService ApiService { get; set; }

        public List<SearchViewModel> Models { get; set; }

        public int LastDownloadId { get; set; }

        public bool _searching = false; 

        protected override void OnInitialized()
        {
            Models = SearchService.SearchResults;
        }

        public async Task Search(string query)
        {
            _searching = true;
 
            // Scroll back to the top
            // Im lazy
            Models = Enumerable.Empty<SearchViewModel>().ToList();
            
            Models = await SearchService.SearchAsync(query);
            _searching = false;
        }

        public async Task SendDownLoadAsync(SearchViewModel model)
        {
            LastDownloadId = await ApiService.DownloadAsync(new DownloadRequest
            {
                DownloadId = model.VideoId,
                Name = model.Title,
                Release = model.Published
            });
        }
    }
}
