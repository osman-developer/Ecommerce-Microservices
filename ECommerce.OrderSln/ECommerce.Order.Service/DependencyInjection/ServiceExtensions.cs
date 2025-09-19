using ECommerce.Order.Domain.Helpers;
using ECommerce.Order.Domain.Interfaces.Clients;
using ECommerce.Order.Domain.Interfaces.Services;
using ECommerce.Order.Service.Clients;
using ECommerce.Order.Service.Services.Core;
using ECommerce.Order.Service.Services.Validations.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace ECommerce.Order.Service.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Autoppaer registeration
            services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);

            // Define resilience policies
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError() // HttpRequestException, 5XX, 408
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * attempt));

            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,   // Break after 5 consecutive failures
                    durationOfBreak: TimeSpan.FromSeconds(30));

            // Register ProductClient with HttpClient + resilience policies
            services.AddHttpClient<IProductClientService, ProductClientService>(client =>
            {
                client.BaseAddress = new Uri(config["Services:ProductServiceBaseUrl"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            // Services
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductValidationService, ProductValidationService>();

            return services;
        }

    }
}
