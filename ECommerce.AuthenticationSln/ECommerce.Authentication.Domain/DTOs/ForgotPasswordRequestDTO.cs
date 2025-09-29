
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Authentication.Domain.DTOs
{
    public record ForgotPasswordRequestDTO(
        [Required] string Email,

        [Required] string Token, 

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        string NewPassword
    );
}
