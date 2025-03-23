using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieBackend.Configurations;
using MovieBackend.Interaces;

namespace MovieBackend.Services
{
    public class AccountService : IAccountService
    {
        private readonly JwtSettings _jwtSettings;

        public AccountService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public Task<object> GetUserInfoAsync(ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
                return Task.FromResult<object>(null);

            var userInfo = new
            {
                Id = user.FindFirstValue(ClaimTypes.NameIdentifier),
                Name = user.FindFirstValue(ClaimTypes.Name),
                Email = user.FindFirstValue(ClaimTypes.Email),
                Picture = user.FindFirstValue("picture")
            };

            return Task.FromResult<object>(userInfo);
        }

        public Task<string> GenerateJwtTokenAsync(ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
                return Task.FromResult<string>(null);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.FindFirstValue(ClaimTypes.NameIdentifier)),
                new Claim(JwtRegisteredClaimNames.Email, user.FindFirstValue(ClaimTypes.Email)),
                new Claim(JwtRegisteredClaimNames.Name, user.FindFirstValue(ClaimTypes.Name)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add picture claim if available
            var picture = user.FindFirstValue("picture");
            if (!string.IsNullOrEmpty(picture))
            {
                claims.Add(new Claim("picture", picture));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<object> GetUserInfoFromExternalLoginAsync(ClaimsPrincipal externalUser)
        {
            var userInfo = new
            {
                Id = externalUser.FindFirstValue(ClaimTypes.NameIdentifier),
                Name = externalUser.FindFirstValue(ClaimTypes.Name),
                Email = externalUser.FindFirstValue(ClaimTypes.Email),
                Picture = externalUser.FindFirstValue("picture")
            };

            return Task.FromResult<object>(userInfo);
        }
    }
}