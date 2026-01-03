namespace JustTouch_Shared.Config
{
    public class MailerSendConfig
    {
        public string FromMail { get; set; } = default!;
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string JustTouchHost { get; set; } = default!;
    }
}
