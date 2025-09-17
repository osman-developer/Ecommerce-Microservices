using ECommerce.Product.Domain.Helpers;
using ECommerce.Product.Domain.Interfaces.Clients;
using ECommerce.Product.Domain.Interfaces.Services;
using ECommerce.Product.Service.Services.Clients;
using ECommerce.Product.Service.Services.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Product.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Autoppaer registeration
            services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(ClientsMappingProfile).Assembly);
           
            // Services

            services.AddScoped<IProductClientService, ProductClientService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

    }
}
