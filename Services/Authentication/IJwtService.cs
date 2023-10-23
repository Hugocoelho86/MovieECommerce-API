using ECommerceMovies.API.Dto;
using Microsoft.AspNetCore.Identity;

namespace ECommerceMovies.API.Services.Authentication
{
    public interface IJwtService
    {
        LoginResponseDto CreateToken(IdentityUser user);
    }
}
