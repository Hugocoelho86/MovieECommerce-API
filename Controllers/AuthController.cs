using ECommerceMovies.API.Dto;
using ECommerceMovies.API.Services.Authentication;
using ECommerceMovies.API.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace ECommerceMovies.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly string role;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IJwtService jwtService, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _emailService = emailService;
            role = "User";
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserRequestDto registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (registerUser.Password != registerUser.ConfirmPassword)
            {
                return BadRequest("Password not equal.");
            }

            var verify = await _userManager.FindByEmailAsync(registerUser.Email);

            if (verify != default)
            {
                return BadRequest("User email is already registered.");
            }

            var newUser = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(newUser, registerUser.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(newUser, role);

            return Created("", newUser.Email);
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                _emailService.SendEmail(new MessageDto ( new List<string> { loginRequest.Email }, "Login Success", "Test" ));
                var token = _jwtService.CreateToken(user);
                return Ok(token);
            }

            return Unauthorized("Invalid Authentication");

        }
    }
}
