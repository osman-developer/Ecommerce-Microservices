using ECommerce.Authentication.Domain.DTOs;
using ECommerce.Authentication.Domain.Interfaces;
using ECommerce.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _identityService.AuthenticateAsync(dto);
            return result != null
                ? Ok(Response<AuthenticatedUserDTO>.Ok(result, "Login successful."))
                : Unauthorized(Response<AuthenticatedUserDTO>.Fail("Invalid credentials."));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(Response<AuthenticatedUserDTO>.Fail("Invalid input."));

            var result = await _identityService.RegisterAsync(dto);
            if (result == null)
                return Conflict(Response<AuthenticatedUserDTO>.Fail("Username or Email already exists."));

            return Ok(Response<AuthenticatedUserDTO>.Ok(result, "Registration successful."));
        }


        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _identityService.ForgotPasswordAsync(email);
            return result.IsEmailSent
                ? Ok(Response<ForgotPasswordConfirmationDTO>.Ok(result, "Password reset email sent."))
                : NotFound(Response<ForgotPasswordConfirmationDTO>.Fail("Email not found."));
        }

      
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordRequestDTO dto)
        {
            var result = await _identityService.ResetPasswordAsync(dto);
            return result != null
                ? Ok(Response<AuthenticatedUserDTO>.Ok(result, "Password reset successful."))
                : BadRequest(Response<AuthenticatedUserDTO>.Fail("Reset password failed."));
        }

       
        [HttpPost("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDTO dto)
        {
            var success = await _identityService.ConfirmEmailAsync(dto);
            return success
                ? Ok(Response<Unit>.Ok("Email confirmed successfully."))
                : BadRequest(Response<Unit>.Fail("Email confirmation failed."));
        }

        [HttpPost("sign-out/{userId}")]
        [Authorize]
        public async Task<IActionResult> SignOut(string userId)
        {
            var success = await _identityService.SignOutAsync(userId);
            return success
                ? Ok(Response<Unit>.Ok("Signed out successfully."))
                : NotFound(Response<Unit>.Fail("User not found or already signed out."));
        }

       
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _identityService.GetUserByIdAsync(userId);
            return result != null
                ? Ok(Response<AuthenticatedUserDTO>.Ok(result, "User retrieved successfully."))
                : NotFound(Response<AuthenticatedUserDTO>.Fail("User not found."));
        }
    }
}
