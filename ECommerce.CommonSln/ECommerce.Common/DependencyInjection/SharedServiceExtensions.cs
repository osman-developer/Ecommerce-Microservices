using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ECommerce.Common.DependencyInjection
{
    public static class SharedServiceExtensions
    {
        public static IServiceCollection AddSerilogLogging(this IServiceCollection services, string logFileName)
        {
            //Configre Serilog Logging
            //if u want to use other than Serilog, just change this block, everything will be working as i depend on ILogger in code
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                    path: $"{logFileName}-.log",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            return services;
        }

        public static IServiceCollection AddAppDbContext<TContext>(this IServiceCollection services, IConfiguration config) where TContext : DbContext
        {
            //Add Generic Database Context
            services.AddDbContextPool<TContext>(option => option.UseSqlServer(
                config.GetConnectionString("DefaultConnection"), sqlserverOption => sqlserverOption.EnableRetryOnFailure()));

            return services;
        }

    }
}
