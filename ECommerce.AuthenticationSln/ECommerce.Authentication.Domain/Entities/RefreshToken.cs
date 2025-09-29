using ECommerce.Common.Entities;

namespace ECommerce.Authentication.Domain.Entities
{
    public class RefreshToken :BaseEntity
    {              
        public string Token { get; init; } = string.Empty;
        public DateTime ExpiresAt { get; init; }
        public bool Revoked { get; init; } = false;
        public string UserId { get; init; } = string.Empty;
        public AppUser AppUser { get; init; } = null!;
    }
}
