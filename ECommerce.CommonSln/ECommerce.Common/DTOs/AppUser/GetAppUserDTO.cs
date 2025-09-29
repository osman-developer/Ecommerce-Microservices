
namespace ECommerce.Common.DTOs.AppUser
{
    public record GetAppUserDTO
    (
        int Id,
        string UserName,
        string Email,
        string Role,
        DateTime? LastLoginAt
    );
}
