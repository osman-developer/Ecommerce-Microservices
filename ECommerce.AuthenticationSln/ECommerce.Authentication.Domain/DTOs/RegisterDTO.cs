
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Authentication.Domain.DTOs
{
    public record RegisterDTO
   (
       [Required(ErrorMessage = "First name is required.")]
        string FirstName,

       [Required(ErrorMessage = "Last name is required.")]
        string LastName,

       [Required(ErrorMessage = "Username is required.")]
        string UserName,

       [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        string Email,

       [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        string Password
   );
}
