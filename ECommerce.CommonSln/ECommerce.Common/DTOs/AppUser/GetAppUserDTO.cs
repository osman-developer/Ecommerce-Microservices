
namespace ECommerce.Common.DTOs.AppUser
{
    public record GetAppUserDTO
    (
        string Id,
        string UserName,
        string Email,
        string Role,
        DateTime? LastLoginAt
    );
}
