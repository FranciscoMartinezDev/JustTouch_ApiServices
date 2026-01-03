using JustTouch_Shared.Config;
using JustTouch_Shared.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace JustTouch_ApiServices.Services
{
    public class MailService
    {
        private SmtpClient smtpClient { get; set; }
        private MailAddress FromAddress { get; set; }

        private readonly MailerSendConfig config;

        public MailService(IOptions<MailerSendConfig> _config) 
        {   
            config  = _config.Value;
            FromAddress = new MailAddress(config.UserName!, "Just Touch");
            smtpClient = new SmtpClient()
            {
                Host = config.Host,
                Port = config.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(config.UserName, config.Password)
            };
        }

        public async Task SendWelcomeMail(Users user)
        {
            var toAddress = new MailAddress(user.Email!, user.UserName);
            string rutaHtml = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "Welcome.html");
            string htmlBody = File.ReadAllText(rutaHtml);
            htmlBody = htmlBody.Replace("{userName}", user.UserName)
                               .Replace("{email}", user.Email!)
                               .Replace("{link}", $"{config.JustTouchHost}/Welcome/{user.Email}")
                               .Replace("{key}", user.Password);

            var message = new MailMessage()
            {
                From = FromAddress,
                Subject = "¡Bienvenido a bordo!",
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(toAddress);
            smtpClient.Send(message);
            await Task.CompletedTask;
        }
    }
}
