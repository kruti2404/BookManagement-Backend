using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthDTOs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManger;
        public JwtService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManger = userManager;
        }


        public async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName)
                };

                var roles = await _userManger.GetRolesAsync(user);
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpireMinutes"])),

                    SigningCredentials = creds,
                    Issuer = _configuration["JwtSettings:Issuer"]
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(jwt);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }


    }
}
