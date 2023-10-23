using System.ComponentModel.DataAnnotations;

namespace ECommerceMovies.API.Dto
{
    public class RegisterUserRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

    }
}
