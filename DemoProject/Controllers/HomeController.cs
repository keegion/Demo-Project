
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DemoProject.Models;
using Microsoft.AspNetCore.Http;
using DemoProject.Hubs;

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

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromForm]Accounts account)
        {
            DemoProjectContext context = new DemoProjectContext();
            if (account.Username != null)
            {
              Accounts dbAcc = context.Accounts.Find(account.Username);
                
                if (dbAcc !=null && dbAcc.Password == account.Password)
                {
                  
                    HttpContext.Session.SetString("username", account.Username);
                    ChatHub chat = new ChatHub();
                   chat.AddUser(account.Username);
                    return View("chat");
                }
                else
                {
                    ViewBag.error = "Wrong Password";
                    return View("index");
                }

            }

            else
            {
             
                    ViewBag.error = "Invalid Account";
                return View("index");

            }
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {

            ChatHub chat = new ChatHub();
            chat.RemoveUser(HttpContext.Session.GetString("username"));
            HttpContext.Session.Remove("username");
            
            return RedirectToAction("index");
        }
      

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromForm]Accounts account)
        {
            DemoProjectContext context = new DemoProjectContext();
            if (account.Username != null)
            {
                if (context.Accounts.Find(account.Username) == null)
                {
                    context.Accounts.Add(account);
                    context.SaveChanges();
                    return View("index");
                }
            }

            ViewBag.error = "Wrong input";
            return View("register");

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
