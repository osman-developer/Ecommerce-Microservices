using AutoMapper;
using ECommerce.Authentication.Domain.DTOs;
using ECommerce.Authentication.Domain.Entities;
using ECommerce.Authentication.Domain.Interfaces;
using ECommerce.Authentication.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Authentication.Service.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly AuthDbContext _dbContext;
        public IdentityService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
             AuthDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        public async Task<AuthenticatedUserDTO?> AuthenticateAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email)
                .ConfigureAwait(false);

            if (user == null) return null;

            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password).ConfigureAwait(false))
                return null;

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var token = _tokenService.GenerateAccessToken(user, roles);

            return _mapper.Map<AuthenticatedUserDTO>(user) with
            {
                Token = token,
                Roles = roles
            };
        }


        public async Task<AuthenticatedUserDTO?> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = _mapper.Map<AppUser>(registerDTO);

            var result = await _userManager.CreateAsync(user, registerDTO.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                // TODO: replace with domain-specific exception or error result
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "Customer").ConfigureAwait(false);

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var token = _tokenService.GenerateAccessToken(user, roles);

            return _mapper.Map<AuthenticatedUserDTO>(user) with
            {
                Token = token,
                Roles = roles
            };
        }

        public async Task<ForgotPasswordConfirmationDTO> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null)
            {
                return new ForgotPasswordConfirmationDTO(email, false, "User not found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

            // TODO: Inject and call IEmailService.SendPasswordResetAsync(user.Email, token)
            return new ForgotPasswordConfirmationDTO(email, true, "Reset password instructions sent.");
        }


        public async Task<AuthenticatedUserDTO?> ResetPasswordAsync(ForgotPasswordRequestDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user == null) return null;

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var token = _tokenService.GenerateAccessToken(user, roles);

            return _mapper.Map<AuthenticatedUserDTO>(user) with
            {
                Token = token,
                Roles = roles
            };
        }

        public async Task<bool> ConfirmEmailAsync(EmailConfirmationDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email).ConfigureAwait(false);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, dto.Token).ConfigureAwait(false);
            return result.Succeeded;
        }

        public async Task<bool> SignOutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null) return false;

            await _signInManager.SignOutAsync().ConfigureAwait(false);

            // Revoke all active refresh tokens for this user
            await RevokeAllRefreshTokensAsync(user.Id).ConfigureAwait(false);

            return true;
        }

       
        private async Task RevokeAllRefreshTokensAsync(string userId)
        {
            var tokens = await _dbContext.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.Revoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync()
                .ConfigureAwait(false);

            if (!tokens.Any()) return;

            foreach (var token in tokens)
                token.Revoked = true;

            _dbContext.RefreshTokens.UpdateRange(tokens);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }



        public async Task<AuthenticatedUserDTO?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            return _mapper.Map<AuthenticatedUserDTO>(user) with { Roles = roles };
        }
    }
}
