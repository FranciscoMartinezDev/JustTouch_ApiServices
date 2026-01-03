using JustTouch_ApiServices.Services;
using JustTouch_ApiServices.SupabaseService;
using JustTouch_Shared.Config;
using Microsoft.Extensions.Options;
using Supabase;

namespace JustTouch_ApiServices.Extentions
{
    public static class ServiceExtention
    {

        public static void AddJustTouchServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SupabaseConfig>(config.GetSection("Supabase"));
            services.AddScoped<Client>(sp =>
            {
                var cfg = sp.GetRequiredService<IOptions<SupabaseConfig>>().Value;
                var options = new SupabaseOptions
                {
                    Schema = "jtouch",
                    AutoRefreshToken = true
                };

                var client = new Client(cfg.Url, cfg.Key, options);
                client.InitializeAsync().GetAwaiter().GetResult();
                return client;
            });
            services.Configure<MailerSendConfig>(config.GetSection("MailerSend"));
            services.AddScoped<MailService>();
            services.AddScoped<ISupabaseRepository, SupabaseRepository>();
        }
    }
}
