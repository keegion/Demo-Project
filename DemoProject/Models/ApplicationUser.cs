using Microsoft.AspNetCore.Identity;
namespace DemoProject.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string ImgSRC { get; set; }
    }
}