using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using XamPass.Models;

namespace XamPass.Controllers
{
    public class AdminController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CreateDB", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel inputModel)
        {
            if (!(inputModel.Username == "admin" && inputModel.Password == "password"))
                return View();
            
            // create claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin")
                //new Claim(ClaimTypes.Email, inputModel.Username)
            };

            // create identity
            ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

            // create principal
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            // sign-in
            await HttpContext.SignInAsync(
                    scheme: "AdminCookieScheme",
                    principal: principal);

            return RedirectToAction("CreateDB", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    scheme: "AdminCookieScheme");

            return RedirectToAction("Login");
        }
    }
}