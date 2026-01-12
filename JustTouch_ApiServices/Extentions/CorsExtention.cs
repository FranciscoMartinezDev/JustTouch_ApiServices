using System.Runtime.CompilerServices;

namespace JustTouch_ApiServices.Extentions
{
    public static class CorsExtention
    {
        public static void AddJTCors(this IServiceCollection services, IConfiguration config)
        {
            string allowed = config["Cors:AllowedOrigin"]!;

            services.AddCors(opt =>
            {
                opt.AddPolicy("Allowed", policy =>
                {
                    policy.WithOrigins([allowed]).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
            });
        }
    }
}
