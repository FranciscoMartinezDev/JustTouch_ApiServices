namespace JustTouch_ApiServices.Helpers
{
    public static class ContentTypeHelper
    {
        public static string GetContentType(string ext) => ext.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}
