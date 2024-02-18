using Minio;

namespace MyMusic.Api.Services
{
    public sealed class UploadService(IMinioClientFactory minioClientFactory)
    {
        public static string BucketName = "songs_done";
    }
}
