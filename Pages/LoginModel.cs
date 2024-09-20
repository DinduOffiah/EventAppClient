﻿using System.ComponentModel.DataAnnotations;

namespace EventAppClient.Pages
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must be between 3 and 50 characters.", MinimumLength = 3)]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password must be up to 6  characters.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
