using MyMusic.Player.Services.Youtube.Models;
using Newtonsoft.Json;

namespace MyMusic.Player.Services.Youtube
{
	public sealed class YoutubeSearchService(IHttpClientFactory httpClientFactory)
	{
		public async Task<YouTubeSearchModel> SearchAsync(string query, CancellationToken cancellationToken)
		{
			try
			{
        using var client = httpClientFactory.CreateClient();

        var request = await client.GetAsync(CreateUrl(query), cancellationToken);

        var response = await request.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<YouTubeSearchModel>(response);
      }
			catch (OperationCanceledException)
			{
				return new() { items = [] };
			}
			catch(Exception)
			{
				throw;
			}
		}

		private static string CreateUrl(string query)
		{
			return $"https://yt.lemnoslife.com/search?part=snippet&q={query}&type=video&order=viewCount";
		}
	}
}
