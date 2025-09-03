using ECommerce.Product.Domain.Interfaces.Services;
using ECommerce.Product.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Product.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // Services
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

    }
}
