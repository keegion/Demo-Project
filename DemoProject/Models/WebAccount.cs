using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoProject.Models
{
    public partial class WebAccount
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
