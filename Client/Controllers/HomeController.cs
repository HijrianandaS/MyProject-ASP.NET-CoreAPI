using Client.Models;
//using Client.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


//jwt
/*using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;*/

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        // Get Action
        public IActionResult Login()
        {
                return View();
            /*//if (HttpContext.Session.GetString("email") == null)
            {
            }
            else
            {
                return RedirectToAction("Index", "Departments");
            }*/
        }





       /* //Post Action
        [HttpPost]
        public IActionResult Login(string email)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                if (ModelState.IsValid)
                {
                    HttpContext.Session.SetString("email", email.ToString());
                    return RedirectToAction("Index", "Departments");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }*/

        //[Authentication]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        /*public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("token");
            return RedirectToAction("Login");
        }*/
    }
}