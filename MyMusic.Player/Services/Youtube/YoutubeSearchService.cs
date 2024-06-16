using MyMusic.Player.Services.Write;
using MyMusic.Player.Services.Youtube.Models;
using Newtonsoft.Json;

namespace MyMusic.Player.Services.Youtube
{
	public sealed class YoutubeSearchService(IHttpClientFactory httpClientFactory, 
		LogWriterService logWriter)
	{
		public async Task<YouTubeSearchModel> SearchAsync(string query, CancellationToken cancellationToken, string NextPageToken = "")
		{
			try
			{
        using var client = httpClientFactory.CreateClient();

        var request = await client.GetAsync(CreateSearchUrl(query, NextPageToken), cancellationToken);

				request.EnsureSuccessStatusCode();

        var response = await request.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<YouTubeSearchModel>(response);
      }
			catch (OperationCanceledException)
			{
				await logWriter.Info("Search has been canceled");
				return null;
			}
			catch(Exception e)
			{
				await logWriter.Error(e, this);
				return null;
			}
		}

		public async Task<YoutubeArtistModel> TryFindArtist(string videoId, CancellationToken cancellationToken)
		{
			try
			{
				using var client = httpClientFactory.CreateClient();

				var request = await client.GetAsync(CreateVideoInfoUrl(videoId), cancellationToken);

				request.EnsureSuccessStatusCode();

				var response = await request.Content.ReadAsStringAsync(cancellationToken);

				if(response.Contains("error"))
				{
					throw new Exception(response);
				}

				return JsonConvert.DeserializeObject<YoutubeArtistModel>(response);
			}
			catch (OperationCanceledException)
			{
				await logWriter.Info("Artists search has been canceled");
				return null;
			}
			catch (Exception e)
			{
				await logWriter.Error(e, this);
				return null;
			}
		}

		private static string CreateSearchUrl(string query, string NextPageToken)
		{
			if (!string.IsNullOrEmpty(NextPageToken))
			{
				return $"https://yt.lemnoslife.com/search?part=snippet&q={query}&type=video&order=relevance&pageToken={NextPageToken}";
			}
			return $"https://yt.lemnoslife.com/search?part=snippet&q={query}&type=video&order=relevance";
		}

		private static string CreateVideoInfoUrl(string videoId)
		{
			return $"https://yt.lemnoslife.com/videos?part=musics&id={videoId}";
		}
	}
}
