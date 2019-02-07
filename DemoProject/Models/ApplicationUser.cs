using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DemoProject.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string ImgSRC { get; set; }
    }
}