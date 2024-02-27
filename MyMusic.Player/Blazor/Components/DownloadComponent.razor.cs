using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services;
using MyMusic.Player.Storage.Models;
using MyMusic.Common.Models;

namespace MyMusic.Player.Blazor.Components
{
    public partial class DownloadComponent : ComponentBase
    {
        [Parameter]
        public MusicReference Model { get; set; }

        [Inject]
        private ApiService ApiService { get; set; }

        [Inject]
        private MusicReferenceService MusicReferenceService { get; set; }


        public async Task SendDownLoadAsync() 
        {
            var request = new DownloadRequest
            {
                Name = Model.Name,
                VideoId = Model.VideoId
            };

            var trackingId = await ApiService.DownloadAsync(request);

            await MusicReferenceService.InsertAsync(new MusicReference 
            {
                TrackingId = trackingId,
                CoverUrl = Model.CoverUrl,
                Name = Model.Name,
            });
        }
    }
}
