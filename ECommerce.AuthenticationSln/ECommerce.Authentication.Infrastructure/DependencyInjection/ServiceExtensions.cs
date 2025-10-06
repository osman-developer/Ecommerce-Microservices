using ECommerce.Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ECommerce.Authentication.Infrastructure.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // Add Database Connectivity
            SharedServiceExtensions.AddAppDbContext<AuthDbContext>(services, config);

            //Add Jwt Authentication Scheme extensions
            JwtAuthenticationExtensions.AddJwtAuthentication(services, config);

            return services;
        }

    }
}
