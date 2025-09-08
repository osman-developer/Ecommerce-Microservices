
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Common.DTOs.AppUser
{
    public record AddOrUpdateAppUserDTO
    (
          int? Id,

        [Required(ErrorMessage = "Name is required.")]
        string Name,

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        string PhoneNumber,

        [Required(ErrorMessage = "Address is required.")]
        string Address,

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        string Email,

        [Required(ErrorMessage = "Password is required.")]
        string Password,

        [Required(ErrorMessage = "Role is required.")]
        string Role
    );
}
