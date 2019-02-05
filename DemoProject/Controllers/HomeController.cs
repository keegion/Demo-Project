
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

namespace DemoProject.Controllers
{
    public class HomeController : Controller
    {

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

            return View("settings");
        }
        [Route("chat")]
        public IActionResult Chat()
        {

            return View("chat");

        }




        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromForm]WebAccount webAcc)
        {
            if (ModelState.IsValid)
            {
                DemoProjectContext context = new DemoProjectContext();
                Accounts dbAcc = context.Accounts.Find(webAcc.Username);
                if (dbAcc != null)
                {
                    if (dbAcc.Password == webAcc.Password)
                    {

                        HttpContext.Session.SetString("username", webAcc.Username);
                        return View("chat");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Wrong Password");
                        return View("index", webAcc);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong username or Connection error");
                    return View("index");
                }


            }

            else
            {

                return View("index");

            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("username");

            return RedirectToAction("index");
        }


        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromForm]RegisterModel RAcc)
        {
            if (ModelState.IsValid)
            {
                DemoProjectContext context = new DemoProjectContext();
                if (context.Accounts.Find(RAcc.Username) == null)
                {
                    Accounts Dacc = new Accounts { Username = RAcc.Username, Email = RAcc.Email, Password = RAcc.Password };

                    context.Accounts.Add(Dacc);
                    context.SaveChanges();
                    return View("index");
                }
                else
                {
                    ModelState.AddModelError("Username", "Username already exist");
                    return View("register", RAcc);
                }
            }

            return View("register");




        }

        private readonly IHostingEnvironment _appEnvironment;

        public HomeController(IHostingEnvironment appEnvironment)

        {
            _appEnvironment = appEnvironment;

        }

        [Route("UploadImage")]
        [HttpPost]

        public async Task<IActionResult> UploadImage(IFormFile file)

        {
            string username = HttpContext.Session.GetString("username");
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
            ChatHub chat = new ChatHub();
            chat.UploadImgToDB(imgPath, username);


            return View("settings");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
