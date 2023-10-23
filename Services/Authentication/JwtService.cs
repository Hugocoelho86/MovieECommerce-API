using ECommerceMovies.API.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceMovies.API.Services.Authentication
{
    public class JwtService: IJwtService
    {

        private const int EXPIRATION_MINUTES = 1;
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponseDto CreateToken(IdentityUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);

            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
                );

            var tokenHandler = new JwtSecurityTokenHandler();

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = expiration
            };
        }

        private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials signingCredentials, DateTime expiration) =>
            new JwtSecurityToken(
                   _configuration["JwtSettings:Issuer"],
                   _configuration["JwtSettings:Audience"],
                   claims,
                   expires: expiration,
                   signingCredentials: signingCredentials
                );

        private Claim[] CreateClaims(IdentityUser user) => new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        private SigningCredentials CreateSigningCredentials() =>
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"])),
                    SecurityAlgorithms.HmacSha256
                );
    }
}
