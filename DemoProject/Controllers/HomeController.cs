
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DemoProject.Models;
using Microsoft.AspNetCore.Http;
using DemoProject.Hubs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.Identity;

namespace DemoProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _appEnvironment;


        public HomeController(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    IHostingEnvironment appEnvironment
                    )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [Route("settings")]
        public IActionResult Settings()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return View("settings");
            }
            else
            {
                return View("index");
            }
        }
        [Route("chat")]
        public IActionResult Chat()
        {

            if (_signInManager.IsSignedIn(User))
            {
                return View("chat");
            }
            else
            {
                return View("index");
            }

        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]WebAccount webAcc)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(webAcc.Username, webAcc.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    return View("chat");
                }
                if (result.IsLockedOut)
                {

                    return View("index");
                }
                else{

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("index");

                }
                
            }
            return View("index");
              
        }


        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.Remove("username");
            await _signInManager.SignOutAsync();
            return RedirectToAction("index");
        }

       [Route("register")]
       [HttpPost]
       public async Task<IActionResult> Register([FromForm]RegisterModel RAcc)
       {
           if (ModelState.IsValid)
           {
                var user = new ApplicationUser { UserName = RAcc.Username, Email = RAcc.Email, PasswordHash = RAcc.Password, ImgSRC = "images/test.png" };
                var result = await _userManager.CreateAsync(user, RAcc.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return View("index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

           return View("register");


       }


        [Route("UploadImage")]
        [HttpPost]
        
        public async Task<IActionResult> UploadImage(IFormFile file)

        {
            string username = _userManager.GetUserName(User);
            if (file == null || file.Length == 0)
                return View("settings");


            string imgPath = "\\Images\\" + username + ".png";
            string path_Root = _appEnvironment.WebRootPath;
            string path_to_Images = path_Root + imgPath;


            using (var stream = new FileStream(path_to_Images, FileMode.Create))

            {

                await file.CopyToAsync(stream);

            }

            ViewData["FilePath"] = path_to_Images;
            if (imgPath != null)
            {
                var user = await _userManager.GetUserAsync(User);
                user.ImgSRC = imgPath;
                await _userManager.UpdateAsync(user);

            }


            return View("settings");
        }
     
        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
