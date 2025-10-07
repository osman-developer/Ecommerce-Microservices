using ECommerce.Common.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Authentication.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public int? CustomerId { get; set; }
    }
}
