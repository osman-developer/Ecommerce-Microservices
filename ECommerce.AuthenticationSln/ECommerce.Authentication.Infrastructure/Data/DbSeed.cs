using ECommerce.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerce.Authentication.Infrastructure.Data
{
    public static class DbSeeder
    {
       
        // Seeds default roles and users into the database.
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                              .CreateLogger("DbSeeder");

            await SeedRolesAsync(roleManager, logger);
            await SeedAdminUserAsync(userManager, logger);
            await SeedDemoCustomerAsync(userManager, logger);
        }


        // Ensures default roles exist.
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            var roles = new[] { "Customer", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (result.Succeeded)
                        logger.LogInformation("Role '{Role}' created successfully.", role);
                }
            }
        }



        // Seeds an admin user if it doesn't exist.
        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, ILogger logger)
        {
            const string adminEmail = "admin@ecommerce.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123$");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Admin user created with email '{Email}'.", adminEmail);
                }
            }
        }

        
        // Seeds a demo customer user if it doesn't exist.
        private static async Task SeedDemoCustomerAsync(UserManager<AppUser> userManager, ILogger logger)
        {
            const string customerEmail = "customer@ecommerce.com";

            if (await userManager.FindByEmailAsync(customerEmail) == null)
            {
                var customer = new AppUser
                {
                    UserName = "demo-customer",
                    Email = customerEmail,
                    FirstName = "Demo",
                    LastName = "Customer",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(customer, "Customer@123$");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, "Customer");
                    logger.LogInformation("Demo customer created with email '{Email}'.", customerEmail);
                }
            }
        }
    }
}
