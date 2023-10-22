using Microsoft.AspNetCore.Components;
using MyMusic.BlazorWasm.Models;
using MyMusic.BlazorWasm.Services;

namespace MyMusic.BlazorWasm.Pages
{
    public partial class Index
    {
        [Inject]
        private IStorageService? storageService { get; set; }

        public ServerConfiguration Model { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Model = new()
            {
                BaseUrl = storageService.GetMyMusicServerURl(),
                Password = storageService.GetServerPassword(),
                DataApiKey = storageService.GetYouTubeDataApiKey()
            };
        }

        public async Task ApplyChanges()
        {
            if (!string.IsNullOrEmpty(Model.Password))
            {
                storageService.SetServerPassword(Model.Password);
            }
            if(!string.IsNullOrEmpty(Model.DataApiKey))
            {
                storageService.SetYouTubeDataApiKey(Model.DataApiKey);
            }

            if (!string.IsNullOrEmpty(Model.BaseUrl))
            {
                storageService.SetMyMusicServerURl(Model.BaseUrl);
            }
        }
    }
}
