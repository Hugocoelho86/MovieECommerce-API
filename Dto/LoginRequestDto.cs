﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceMovies.API.Dto
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
