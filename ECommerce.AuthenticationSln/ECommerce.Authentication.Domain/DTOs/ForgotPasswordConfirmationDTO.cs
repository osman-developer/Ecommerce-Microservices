
namespace ECommerce.Authentication.Domain.DTOs
{

    public record ForgotPasswordConfirmationDTO(
        string Email,
        bool IsEmailSent,
        string? Message = null
    );
}
