using ECommerceMovies.API.Dto;
using ECommerceMovies.API.Models;
using ECommerceMovies.API.Services.Authentication;
using ECommerceMovies.API.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using MimeKit;
using System.Net.Mail;
using System.Text;

namespace ECommerceMovies.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly string role;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IJwtService jwtService, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _emailService = emailService;
            role = "User";
            _configuration = configuration;
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

            var newUser = new User
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

            // Send email to confirme account
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            var url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?useremail={newUser.Email}&token={validEmailToken}";

            var mailAdress = new MailboxAddress(newUser.UserName, newUser.Email);
            string subject = "Confirm your email account";
            string body = $"<p>Please confirm your email by <a href='{url}'>Click here</a></p>";

            await _emailService.SendEmailAsync(new MessageDto(mailAdress, subject, body));

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
                var token = _jwtService.CreateToken(user);
                return Ok(token);
            }

            return Unauthorized("Invalid Authentication");
        }

        [HttpGet("confirmemail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmail(string userEmail, string token)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == default)
            {
                return NotFound();
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);


            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok("Email account confirmed");
        }

        [HttpGet("forgetpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == default)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(token);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
            //Url Front-end page for password reset
            var url = $"{_configuration["AppUrl"]}/resetemail?useremail={email}&token={validEmailToken}";

            string subject = "Confirm your email account";
            string body = $"<p>To reset your password <a href='{url}'>Click here</a></p>";

            await _emailService.SendEmailAsync(new MessageDto(email, subject, body));

            return Ok();
        }

        [HttpPost("resetemail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResetEmail(ResetEmailRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!request.Password.Equals(request.ConfirmPassword))
            {
                return BadRequest("The password's are not the same");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == default)
            {
                return NotFound();
            }

            var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok("Password changed");
        }
    }
}
