using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using XamPass.Models;
using XamPass.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XamPass.Controllers
{    
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult CreateDB()
        {
            DBInitialize.DatabaseTest(_context, true);
            return RedirectToAction("Done");
        }

        #region Tests

        [HttpGet]
        public async Task<IActionResult> FirstTest()
        {
            // Liste aller DtUniversity-Objekte wird aus DB erstellt
            var universities = await _context.Universities.ToListAsync();
            // ViewModel wird instanziiert
            var model = new FirstModel();
            // im ViewModel wird eine Liste aus SelectListItems erstellt
            model.Institutions = new List<SelectListItem>();
            // SelectListItems werden aus DtUniversity-Objekten erszeugt und Liste hinzugefügt
            // SelectListItem besteht aus Value = ID und Text = Name
            foreach (var item in universities)
            {
                model.Institutions.Add(new SelectListItem() { Value = item.UniversityID.ToString(), Text = item.UniversityName });
            }
            //var result = new ViewModelSearch();
            //result.Universities = model.Institutions;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FirstTest(FirstModel model)
        {
            // ViewModel wird als Parameter entgegengenommen
            // Liste der DtUniversity-Objekte wird auf passende ID durchsucht
            // bei Treffer wird auf Website "/Home/Done" umgeleitet und gefundene Institution mitgegeben
            var universities = await _context.Universities.ToListAsync();
            if (ModelState.IsValid)
            {
                var university = universities.First(u => u.UniversityID == model.InstitutionId);
                var result = new Institution() { Id = (int)university.UniversityID, Name = university.UniversityName };
                var viewModelSearch = new ViewModelSearch();
                viewModelSearch.UniversityId = result.Id;
                return RedirectToAction("Done", viewModelSearch);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SecondTest()
        {
            var universities = await _context.Universities.ToListAsync();
            var model = new SecondModel();
            model.InstitutionList = new List<Institution>();
            foreach (var item in universities)
            {
                model.InstitutionList.Add(new Institution() { Id = (int)item.UniversityID, Name = item.UniversityName });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SecondTest(string institution)
        {
            var universities = await _context.Universities.ToListAsync();
            var model = new SecondModel();
            model.InstitutionList = new List<Institution>();
            if (ModelState.IsValid)
            {
                var uni = universities.First(u => u.UniversityName == institution);
                var result = new Institution() { Id = (int)uni.UniversityID, Name = uni.UniversityName };
                model.InstitutionList.Add(result);
                var viewModelSearch = new ViewModelSearch();
                viewModelSearch.UniversityId = result.Id;
                return RedirectToAction("Done", viewModelSearch);
            }
            return View(model);
        }

        #endregion

        public IActionResult Done(ViewModelSearch viewModelSearch)
        {
            var result = viewModelSearch;
            return View(result);
        }

        #region Authorization
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
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

            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    scheme: "AdminCookieScheme");

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}