using ECommerce.Authentication.Domain.Helpers;
using ECommerce.Authentication.Domain.Interfaces;
using ECommerce.Authentication.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Authentication.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Autoppaer registeration
            services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
