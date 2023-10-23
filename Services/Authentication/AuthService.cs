using Microsoft.AspNetCore.Identity;

namespace ECommerceMovies.API.Services.Authentication
{
    public class AuthService
    {
        private const int EXPIRATION_MINUTES = 1;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }



    }
}
