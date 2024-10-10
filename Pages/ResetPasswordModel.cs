﻿using System.ComponentModel.DataAnnotations;

namespace EventAppClient.Pages
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "New Password must be at least 6 characters.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is required.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}