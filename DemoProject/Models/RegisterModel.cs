using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoProject.Models
{
    public partial class RegisterModel
    {
        [Required, MaxLength(30)]
        public string Username { get; set; }

        [Required, MaxLength(30), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(30), DataType(DataType.Password)]

        public string Password { get; set; }

        [Required, DataType(DataType.Password), MaxLength(30)]
        [Compare("Password", ErrorMessage = "The password does not mach")]
        public string ConfirmPassword { get; set; }
    }
}
