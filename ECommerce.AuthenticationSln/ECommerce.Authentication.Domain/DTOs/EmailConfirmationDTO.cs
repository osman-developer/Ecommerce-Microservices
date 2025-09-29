using System.ComponentModel.DataAnnotations;


namespace ECommerce.Authentication.Domain.DTOs
{
    public record EmailConfirmationDTO(
         [Required] string Email,

         [Required] string Token 
     );
}
