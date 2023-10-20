using MyMusic.BlazorWasm.Models;
using MyMusic.Common;

namespace MyMusic.BlazorWasm.Pages
{
    public partial class Index
    {
        public ServerConfiguration Model { get; set; } = new() { BaseUrl = "http://localhost:5248", DataApiKey = EnviromentProvider.GetDataApiKey() };

        public async Task ApplyChanges()
        {
            
        }
    }
}
