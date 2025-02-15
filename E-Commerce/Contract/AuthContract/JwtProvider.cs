using E_Commerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Contract.AuthContract
{
    public class JwtProvider : IJwtProvider
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtProvider(string secretKey, string issuer, string audience, UserManager<ApplicationUser> userManager)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _userManager = userManager;
        }

        public async Task<JwtResponse> GenerateTokenAsync(ApplicationUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),  // Subject claim for the user ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  // Unique identifier for the token
            };

            var roles = await _userManager.GetRolesAsync(user.Id);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = 30 * 60  // 30 minutes in seconds
            };
        }


    }
}
