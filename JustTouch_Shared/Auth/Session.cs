namespace JustTouch_Shared.Auth
{
    public class Session
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? TokenType { get; set; }
        public long ExpiresIn { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
