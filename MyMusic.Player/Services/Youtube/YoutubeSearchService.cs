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

        var request = await client.GetAsync(CreateSearchUrl(query), cancellationToken);

        var response = await request.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<YouTubeSearchModel>(response);
      }
			catch (OperationCanceledException)
			{
				return null;
			}
			catch(Exception)
			{
				throw;
			}
		}

		public async Task<YoutubeArtistModel> TryFindArtist(string videoId, CancellationToken cancellationToken)
		{
			try
			{
				using var client = httpClientFactory.CreateClient();

				var request = await client.GetAsync(CreateVideoInfoUrl(videoId), cancellationToken);

				var response = await request.Content.ReadAsStringAsync(cancellationToken);

				return JsonConvert.DeserializeObject<YoutubeArtistModel>(response);
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static string CreateSearchUrl(string query)
		{
			return $"https://yt.lemnoslife.com/search?part=snippet&q={query}&type=video&order=viewCount";
		}

		private static string CreateVideoInfoUrl(string videoId)
		{
			return $"https://yt.lemnoslife.com/videos?part=musics&id={videoId}";
		}
	}
}
