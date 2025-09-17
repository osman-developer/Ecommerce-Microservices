using ECommerce.Order.Domain.Helpers;
using ECommerce.Order.Domain.Interfaces.Clients;
using ECommerce.Order.Domain.Interfaces.Services;
using ECommerce.Order.Service.Clients;
using ECommerce.Order.Service.Services.Core;
using ECommerce.Order.Service.Services.Validations.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace ECommerce.Order.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Autoppaer registeration
            services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);

            // Register ProductClient with HttpClient + retry policy
            services.AddHttpClient<IProductClientService, ProductClientService>(client =>
            {
                client.BaseAddress = new Uri(config["Services:ProductServiceBaseUrl"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            })
            .AddPolicyHandler(Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(msg => !msg.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200 * retry)));


            // Services
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductValidationService, ProductValidationService>();

            return services;
        }

    }
}
