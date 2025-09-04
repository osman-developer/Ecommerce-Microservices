using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Common.DependencyInjection
{
    public static class SharedServiceExtensions
    {
      
        public static IServiceCollection AddAppDbContext<TContext>(this IServiceCollection services, IConfiguration config) where TContext : DbContext
        {
            //Add Generic Database Context
            services.AddDbContextPool<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("DefaultConnection"), sqlserverOption => sqlserverOption.EnableRetryOnFailure()));

            return services;
        }

    }
}
