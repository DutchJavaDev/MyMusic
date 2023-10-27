namespace MyMusic.Api
{
    public static class Utils
    {
        public enum PipeLineState : int
        {
            Null,
            Created,
            Downloaded,
            Converted,
            Uploading,
            Done,
            Failed
        }
    }
}
