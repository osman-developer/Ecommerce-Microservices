using ECommerce.Order.Domain.Helpers;
using ECommerce.Order.Domain.Interfaces.Services;
using ECommerce.Order.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Order.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Autoppaer registeration
            services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);

            // 3rd party 
            services.AddHttpClient();

            // Services
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }

    }
}
