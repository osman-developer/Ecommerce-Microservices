using ECommerce.Authentication.Domain.DTOs;

namespace ECommerce.Authentication.Domain.Interfaces
{
    public interface IIdentityService
    {
       
        Task<AuthenticatedUserDTO> AuthenticateAsync(LoginDTO loginDTO);

        Task<AuthenticatedUserDTO> RegisterAsync(RegisterDTO registerDTO);

        Task<ForgotPasswordConfirmationDTO> ForgotPasswordAsync(string email);

        Task<AuthenticatedUserDTO> ResetPasswordAsync(ForgotPasswordRequestDTO forgotPasswordRequestDTO);

        Task<bool> ConfirmEmailAsync(EmailConfirmationDTO emailConfirmationDTO);

        Task<bool> SignOutAsync(string userId);

        Task<AuthenticatedUserDTO> GetUserByIdAsync(string userId);
    }
}
