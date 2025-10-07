using ECommerce.Authentication.Domain.Entities;
using ECommerce.Common.DependencyInjection;
using Microsoft.AspNetCore.Identity;
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

            //Add ASP.NET Identity setup
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // ─ Password Policy ──────────────────────────────
                options.Password.RequiredLength = 12;                // Minimum 12 characters
                options.Password.RequireDigit = true;                // Must include at least one number
                options.Password.RequireNonAlphanumeric = true;     // Must include symbols
                options.Password.RequireUppercase = true;           // Must include uppercase
                options.Password.RequireLowercase = true;           // Must include lowercase

                // ─ Lockout Settings ────────────────────────────
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);  // Lockout duration
                options.Lockout.MaxFailedAccessAttempts = 5;                        // Max failed attempts
                options.Lockout.AllowedForNewUsers = true;

                // ─ User Settings ───────────────────────────────
                options.User.RequireUniqueEmail = true;             // Ensure emails are unique
                options.SignIn.RequireConfirmedEmail = true;       // Force email confirmation before login

            })
            .AddEntityFrameworkStores<AuthDbContext>()              
            .AddDefaultTokenProviders();                             

            //Add Jwt Authentication Scheme extensions
            JwtAuthenticationExtensions.AddJwtAuthentication(services, config);

            return services;
        }

    }
}
