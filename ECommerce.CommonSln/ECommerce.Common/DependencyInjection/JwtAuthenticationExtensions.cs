using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.Common.DependencyInjection
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var key = config["JWTAuthentication:Key"] ?? throw new ArgumentNullException("JWTAuthentication:Key is missing");
            var issuer = config["JWTAuthentication:Issuer"] ?? throw new ArgumentNullException("JWTAuthentication:Issuer is missing");
            var audience = config["JWTAuthentication:Audience"] ?? throw new ArgumentNullException("JWTAuthentication:Audience is missing");

            var encodedKey = Encoding.UTF8.GetBytes(key);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true; // enforce HTTPS in production
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // ensure expired tokens are rejected (refresh token)
                        ClockSkew = TimeSpan.Zero, // no tolerance
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(encodedKey)
                    };
                });

            return services;
        }
    }
}
