using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoProject.Models;
using Microsoft.AspNetCore.Http;

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

        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromForm]Accounts account)
        {
            DemoProjectContext context = new DemoProjectContext();
            if (account.Username != null)
            {
                Accounts dbAcc = context.Accounts.Find(account.Username);
                if (dbAcc.Password == account.Password)
                {
                    HttpContext.Session.SetString("username", account.Username);
                    return View("chat");
                }
                else
                {
                    ViewBag.error = "Wrong Password";
                    return View("login");
                }

            }

            else
            {
                if (context != null)
                {
                    ViewBag.error = "Invalid Account  + c";
                }
                else
                    ViewBag.error = "Invalid Account";
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
