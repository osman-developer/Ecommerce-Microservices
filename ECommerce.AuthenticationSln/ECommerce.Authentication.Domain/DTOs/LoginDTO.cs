using System.ComponentModel.DataAnnotations;

namespace ECommerce.Authentication.Domain.DTOs
{
    public record LoginDTO
   (
       [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        string Email,

       [Required(ErrorMessage = "Password is required.")]
        string Password
   );
}
