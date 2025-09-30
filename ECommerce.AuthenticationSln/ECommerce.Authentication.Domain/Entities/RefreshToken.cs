using ECommerce.Common.Entities;

namespace ECommerce.Authentication.Domain.Entities
{
    public class RefreshToken :BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool Revoked { get; set; } = false;
        public string UserId { get; set; } = string.Empty;
        public AppUser AppUser { get; set; } = null!;
    }
}
