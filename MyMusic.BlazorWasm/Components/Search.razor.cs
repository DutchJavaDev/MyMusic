using Microsoft.AspNetCore.Components;
using MyMusic.BlazorWasm.Models;
using MyMusic.BlazorWasm.Models.Search;
using MyMusic.BlazorWasm.Services;

namespace MyMusic.BlazorWasm.Components
{
    public partial class Search
    {
        [Inject]
        private SearchService searchService { get; set; }

        private SearchModel Model { get; set; } = new();

        private List<SearchResult> searchResultsVideo { get; set; }

        private SearchResult? rppto { get; set; } = new();

        private string? Res { get; set; } = string.Empty;


        private async Task SearchAsync()
        {
            await searchService.Search2(Model.Query);

            Res = searchService.Response;
            rppto = searchService.Root;

            StateHasChanged();
        }
    }
}
