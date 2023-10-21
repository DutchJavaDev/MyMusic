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

        private async Task SearchAsync()
        {
            await searchService.Search2(Model.Query);
        }
    }
}
