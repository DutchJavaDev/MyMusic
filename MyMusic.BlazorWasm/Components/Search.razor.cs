using Microsoft.AspNetCore.Components;
using MyMusic.BlazorWasm.Models;
using MyMusic.BlazorWasm.Models.Search;
using MyMusic.BlazorWasm.Services;

namespace MyMusic.BlazorWasm.Components
{
    public partial class Search
    {
        [Inject]
        private SearchService? searchService { get; set; }

        private SearchModel Model { get; set; } = new();

        private IEnumerable<SearcViewModel>? Result { get; set; }

        private async Task SearchAsync()
        {
            Result = await searchService.Search(Model.Query);
        }
    }
}
