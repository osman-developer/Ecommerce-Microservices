using ECommerce.Authentication.Domain.Helpers;
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
            return services;
        }
    }
}
