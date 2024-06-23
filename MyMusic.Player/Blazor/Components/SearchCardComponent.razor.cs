using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MyMusic.Player.Services;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Services.Write;
using MyMusic.Player.Services.Youtube;
using MyMusic.Player.Services.Youtube.Models;
using MyMusic.Player.Storage.Models;
using static MyMusic.Common.CommonData;

namespace MyMusic.Player.Blazor.Components
{
	public partial class SearchCardComponent : ComponentBase, IDisposable
	{
		private bool disposedValue;
		private readonly CancellationTokenSource _tokenSource = new();

		private bool IsBusy { get; set; } = false;
		[Inject]
		public ApiService ApiService { get; set; }
		[Parameter]
		public Item Model { get; set; }
		[Parameter]
		public LogWriterService LogWriterService { get; set; }
		[Parameter]
		public YoutubeSearchService YoutubeSearchService { get; set; }
		[Parameter]
		public ArtistWriterService ArtistWriterService { get; set; }
		[Parameter]
		public ArtistReaderService ArtistReaderService { get; set; }
		[Parameter]
		public SongWriterService SongWriterService { get; set; }
		[Parameter]
		public SongStatusWriterService SongStatusWriterService { get; set; }

		private async Task SendDownloadRequest()
		{
			IsBusy = true;

			var title = Model.snippet.title;
			// try and grab artist
			var artistInfoResult = await YoutubeSearchService.TryFindArtist(Model.id.videoId, _tokenSource.Token);

			var artistFound = true;

			if (artistInfoResult == null || 
				  artistInfoResult.items.FirstOrDefault() == null ||
					artistInfoResult?.items?.FirstOrDefault()?.musics.FirstOrDefault() == null ||
					string.IsNullOrEmpty(artistInfoResult?.items?.FirstOrDefault()?.musics?.FirstOrDefault()?.artist))
			{
				await LogWriterService.Error($"{nameof(artistInfoResult)} result is empty", $"Could not find artist for {Model.snippet.title}, using channel instead.");
				IsBusy = false;
				artistFound = false;
			}

			IsBusy = false;
			var name = string.Empty;
			Music? music = null;

			if (artistFound)
			{
				music = artistInfoResult.items.FirstOrDefault().musics.FirstOrDefault();
				name = music.artist;
				AppNotification.Success("Found artist", $"Found artist for song {title} || {music.artist}");
			}
			else
			{
				// Use channel Id
				name = Model.snippet.channelTitle;
			}

			// check if artist exists
			var artist = await ArtistReaderService.GetArtistsByNameAsync(name);

			// if not add
			if (artist == null)
			{
				await ArtistWriterService.AddArtistAsync(new()
				{
					ImageUrl = artistFound ? music.image : Model.snippet.channelThumbnails.First().url, // replace with remote url?
					Name = name,
				});

				artist = await ArtistReaderService.GetArtistsByNameAsync(name);
			}

			// Insert song with artist serial
			var song = new Song() 
			{
				ArtistsSerial = artist.Serial,
				Title = artistFound ? music.song : Model.snippet.title,
				Duration = Model.snippet.duration,
			};

			await SongWriterService.InsertSongAsync(song);

			// Send api request
			var trackId = await ApiService.DownloadAsync(new() {
				Name = name,
				VideoId = Model.id.videoId,
				Release = DateTime.Now, // TODO fetch real release
			});

			// Inser songstatus
			await SongStatusWriterService.InsertStatus(new () {
				TrackingId = trackId,
				SongSerial = song.Serial,
				Status = (int)Mp3State.Created
			});
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_tokenSource.Cancel();
					_tokenSource.Dispose();
				}

				disposedValue = true;
			}
		}

		// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		~SearchCardComponent()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
