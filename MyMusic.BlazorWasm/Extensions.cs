using MyMusic.BlazorWasm.Models.Search;

namespace MyMusic.BlazorWasm
{
    public static class Extensions
    {
        public static List<SearcViewModel> ToViewModels(this List<Item> result)
        {
            return result.Select(i => new SearcViewModel
            {
                Title = i.snippet.title,
                Description = i.snippet.description,
                CoverUrl = i.snippet.thumbnails.medium.url,
                VideoId = i.id.videoId
            }).ToList();
        }
    }
}
