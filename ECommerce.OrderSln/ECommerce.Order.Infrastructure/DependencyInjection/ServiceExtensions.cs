using ECommerce.Common.DependencyInjection;
using ECommerce.Common.Interface.Repository;
using ECommerce.Order.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Order.Infrastructure.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Database Connectivity
            SharedServiceExtensions.AddAppDbContext<OrderDbContext>(services, config);

            //Add Jwt Authentication Scheme extensions
            JwtAuthenticationExtensions.AddJwtAuthentication(services, config);

            // Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

    }
}
