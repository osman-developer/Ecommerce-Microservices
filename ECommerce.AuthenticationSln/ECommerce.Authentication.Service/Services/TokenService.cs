using ECommerce.Authentication.Domain.Entities;
using ECommerce.Authentication.Domain.Interfaces;
using ECommerce.Authentication.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Authentication.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;

        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _accessTokenExpiryMinutes;
        private readonly int _refreshTokenExpiryDays;

        public TokenService(
            AuthDbContext dbContext,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tokenValidationParameters = tokenValidationParameters;

            _jwtKey = _configuration["JWTAuthentication:Key"]!;
            _jwtIssuer = _configuration["JWTAuthentication:Issuer"]!;
            _jwtAudience = _configuration["JWTAuthentication:Audience"]!;
            _accessTokenExpiryMinutes = int.Parse(_configuration["JWTAuthentication:AccessTokenExpiryMinutes"] ?? "60");
            _refreshTokenExpiryDays = int.Parse(_configuration["JWTAuthentication:RefreshTokenExpiryDays"] ?? "14");
        }

        public string GenerateAccessToken(AppUser user, IList<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        public Guid? ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
                var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
                return Guid.TryParse(userId, out var guid) ? guid : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(AppUser user)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays),
                UserId = user.Id,
                AppUser = user
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return refreshToken;
        }

        public async Task RevokeRefreshToken(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token == null || token.Revoked) return;

            token.Revoked = true;
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}
