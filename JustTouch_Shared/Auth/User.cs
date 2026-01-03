namespace JustTouch_Shared.Auth
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, object>? UserMetadata { get; set; }
    }
}
