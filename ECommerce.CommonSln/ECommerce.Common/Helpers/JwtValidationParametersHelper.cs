
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.Common.Helpers
{
    public static class JwtValidationParametersHelper
    {
        public static TokenValidationParameters GetParameters(IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config["JWTAuthentication:Key"]!);
            var issuer = config["JWTAuthentication:Issuer"]!;
            var audience = config["JWTAuthentication:Audience"]!;

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
