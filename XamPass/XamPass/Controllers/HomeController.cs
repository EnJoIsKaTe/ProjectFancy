using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XamPass.Models;
using XamPass.Models.DataBaseModels;

namespace XamPass.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var viewModelSearch = GetViewModelSearch().Result;
                return View(viewModelSearch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("CreateDB");
            }
        }

        [HttpPost]
        public IActionResult Index(ViewModelSearch viewModelSearch)
        {
            var result = viewModelSearch;
            var questions = _context.Questions.ToList();
            var uniList = _context.Universities.ToList();
            var resultList = new List<DtUniversity>();
            foreach (var item in questions)
            {
                var uniId = item.UniversityID;
                resultList.Add(uniList.FirstOrDefault(u => u.UniversityID == uniId));
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in resultList)
            {
                sb.AppendLine(item.UniversityName);
            }
            ViewBag.Test = sb.ToString();
            return View(viewModelSearch);
        }

        private async Task<ViewModelSearch> GetViewModelSearch()
        {
            var universities = await _context.Universities.ToListAsync();
            var federalStates = await _context.FederalStates.ToListAsync();
            var subjects = await _context.Subjects.ToListAsync();
            var fieldsOfStudies = await _context.FieldsOfStudies.ToListAsync();

            var viewModelSearch = new ViewModelSearch();

            foreach (var item in universities)
            {
                viewModelSearch.Universities.Add(new SelectListItem()
                {
                    Value = item.UniversityID.ToString(),
                    Text = item.UniversityName
                });
            }
            foreach (var item in federalStates)
            {
                viewModelSearch.FederalStates.Add(new SelectListItem()
                {
                    Value = item.FederalStateID.ToString(),
                    Text = item.FederalStateName
                });
            }
            foreach (var item in subjects)
            {
                viewModelSearch.Subjects.Add(new SelectListItem()
                {
                    Value = item.SubjectID.ToString(),
                    Text = item.SubjectName
                });
            }
            foreach (var item in fieldsOfStudies)
            {
                viewModelSearch.FieldsOfStudies.Add(new SelectListItem()
                {
                    Value = item.FieldOfStudiesID.ToString(),
                    Text = item.FieldOfStudiesName
                });
            }
            return viewModelSearch;
        }

        [Authorize]
        public IActionResult CreateDB()
        {
            // Solange der DB-Server noch nicht bereit ist sollte dieser Methodenaufruf auskommentiert sein
            DBInitialize.DatabaseTest(_context);
            return RedirectToAction("Done");
        }

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
                return RedirectToAction("Done", result);
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
                return RedirectToAction("Done", result);
            }
            return View(model);
        }

        public IActionResult Done(Institution institution)
        {
            return View(institution);
        }

        #region template-methods
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
