using MyMusic.BlazorWasm.Models;

namespace MyMusic.BlazorWasm.Pages
{
    public partial class Index
    {
        public ServerConfiguration Model { get; set; } = new() { BaseUrl = "http://localhost:5248" };

        public async Task ApplyChanges()
        {
            
        }
    }
}
