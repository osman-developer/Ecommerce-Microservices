using ECommerce.Authentication.Domain.Entities;

namespace ECommerce.Authentication.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser user, IList<string> roles);
        string GenerateRefreshToken();
        Guid? ValidateToken(string token);
        /// Revokes a refresh token (used in SignOut or security events).
        Task RevokeRefreshToken(string refreshToken);
    }
}
