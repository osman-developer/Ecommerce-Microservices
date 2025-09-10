
namespace ECommerce.Common.DTOs.AppUser
{
    public record GetAppUserDTO
   (
       int Id,
       string Name,
       string PhoneNumber,
       string Address,
       string Email,
       string Role
   );
}
