using ECommerce.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Authentication.Infrastructure.Configurations
{
    public static class IdentityTableNamesSetup
    {
        public static void ConfigureTables(ModelBuilder builder)
        {
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<IdentityRole<string>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}
