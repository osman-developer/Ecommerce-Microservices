using ECommerce.Order.Domain.Helpers;
using ECommerce.Order.Domain.Interfaces.Clients;
using ECommerce.Order.Domain.Interfaces.Services;
using ECommerce.Order.Service.Clients;
using ECommerce.Order.Service.Services.Clients;
using ECommerce.Order.Service.Services.Core;
using ECommerce.Order.Service.Services.Validations.AppUser;
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
            // Define resilience policies
            var retryPolicy = GetRetryPolicy();
            var circuitBreakerPolicy = GetCircuitBreakerPolicy();

            // Register ProductClient with HttpClient + resilience policies
            services.AddHttpClient<IProductClientService, ProductClientService>(client =>
            {
                client.BaseAddress = new Uri(config["Services:ProductServiceBaseUrl"]);
                client.Timeout = TimeSpan.FromSeconds(10);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            // Register AppUserClient with HttpClient + resilience policies
            services.AddHttpClient<IAppUserClientService, AppUserClientService>(client =>
            {
                client.BaseAddress = new Uri(config["Services:AuthServiceBaseUrl"]); // <-- Auth service base URL
                client.Timeout = TimeSpan.FromSeconds(10);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(circuitBreakerPolicy);

            // Add Autoppaer registeration
            services.AddAutoMapper(cfg => { }, typeof(MappingProfiles).Assembly);

            // Services
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductValidationService, ProductValidationService>();
            services.AddScoped<IAppUserValidationService, AppUserValidationService>();

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * attempt));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

    }
}
