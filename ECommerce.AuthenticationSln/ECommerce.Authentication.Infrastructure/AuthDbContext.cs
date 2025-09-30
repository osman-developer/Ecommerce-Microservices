using ECommerce.Authentication.Domain.Entities;
using ECommerce.Authentication.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Authentication.Infrastructure
{
    public class AuthDbContext : IdentityDbContext<AppUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Rename default Identity tables
            IdentityTableNamesSetup.ConfigureTables(builder);

            // Apply RefreshToken configuration
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}