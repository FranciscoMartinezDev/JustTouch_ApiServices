using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JustTouch_ApiServices.Extentions
{
    public static class AuthenticationExtention
    {
        public static void AddJustTouchAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var supabaseUrl = config["Supabase:Url"];
            var supabaseAnonKey = config["Supabase:Key"];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        string authority = config["Supabase:AuthHost"]!.Substring(0, config["Supabase:AuthHost"]!.Length - 1);
                        string secret = config["Supabase:JWT"]!;

                        options.Authority = authority;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidIssuer = authority,

                            ValidateAudience = false,
                            ValidAudience = "authenticated",

                            ValidateLifetime = true,
                            RequireExpirationTime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                // Si NO viene token → no marcar error → dejar que el endpoint decida
                                if (!context.Request.Headers.ContainsKey("Authorization"))
                                {
                                    context.NoResult();
                                }

                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddAuthorization();
        }
    }
}
