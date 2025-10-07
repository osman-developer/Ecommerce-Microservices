using ECommerce.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Common.DependencyInjection
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var parameters = JwtValidationParametersHelper.GetParameters(config);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // For dev
                options.SaveToken = true;
                options.TokenValidationParameters = parameters;
            });

            // Register the same parameters for DI in services like TokenService
            services.AddSingleton(parameters);


            return services;
        }
    }
}
