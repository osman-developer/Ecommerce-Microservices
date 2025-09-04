using ECommerce.Common.DependencyInjection;
using ECommerce.Common.Interface.Repository;
using ECommerce.Product.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Product.Infrastructure.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Database Connectivity
            SharedServiceExtensions.AddAppDbContext<ProductDbContext>(services, config);

            //Add Jwt Authentication Scheme extensions
            JwtAuthenticationExtensions.AddJwtAuthentication(services, config);

            // Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }

    }
}
