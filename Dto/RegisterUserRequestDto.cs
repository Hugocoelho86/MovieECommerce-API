﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceMovies.API.Dto
{
    public class RegisterUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

    }
}
