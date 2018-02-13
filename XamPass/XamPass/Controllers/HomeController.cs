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
            // var result = viewModelSearch;
            //var questions = _context.Questions.ToList();
            var resultList = new List<DtQuestion>();


            // Alle Fragen werden aus der Datenbank geladen und danach mit den eingegebenen Filtern durchsucht
            resultList = _context.Questions.ToList();

            if (viewModelSearch.UniversityId != 0)
            {
                resultList = resultList.Where(q => q.UniversityID == viewModelSearch.UniversityId).ToList();
            }

            if (viewModelSearch.FieldOfStudiesId != 0)
                resultList = resultList.Where(q => q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId).ToList();

            if (viewModelSearch.SubjectId != 0)
                resultList = resultList.Where(q => q.SubjectID == viewModelSearch.SubjectId).ToList();

            //if (viewModelSearch.UniversityId == 0)
            //{
            //    resultList = questions;
            //}
            //else
            //{
            //    resultList = questions.Select(q => q)
            //        .Where(q => q.UniversityID == viewModelSearch.UniversityId).ToList();
            //}

            StringBuilder sb = new StringBuilder();
            foreach (var item in resultList)
            {
                sb.AppendLine(item.Content + ";");
            }
            ViewBag.Test = sb.ToString();
            viewModelSearch = GetViewModelSearch().Result;
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

        #region Add new Question

        [HttpPost]
        public IActionResult CreateQuestion(ViewModelSearch viewModelSearch)
        {
            var result = viewModelSearch;

            ViewModelCreate vieModelCreate = new ViewModelCreate(viewModelSearch);
            return RedirectToAction("Done", result);
        }

        /// <summary>
        /// Lädt Vorschläge für die Properties der neuen Frage aus der Datenbank
        /// </summary>
        /// <returns></returns>
        //private async Task<ViewModelCreate> GetViewModelCreate()
        //{
        //    var universities = await _context.Universities.ToListAsync();
        //    var federalStates = await _context.FederalStates.ToListAsync();
        //    var subjects = await _context.Subjects.ToListAsync();
        //    var fieldsOfStudies = await _context.FieldsOfStudies.ToListAsync();

        //    var viewModelCreate = new ViewModelCreate();

        //    foreach (var item in universities)
        //    {
        //        viewModelCreate.Universities.Add(new SelectListItem()
        //        {
        //            Value = item.UniversityID.ToString(),
        //            Text = item.UniversityName
        //        });
        //    }
        //    foreach (var item in federalStates)
        //    {
        //        viewModelCreate.FederalStates.Add(new SelectListItem()
        //        {
        //            Value = item.FederalStateID.ToString(),
        //            Text = item.FederalStateName
        //        });
        //    }
        //    foreach (var item in subjects)
        //    {
        //        viewModelCreate.Subjects.Add(new SelectListItem()
        //        {
        //            Value = item.SubjectID.ToString(),
        //            Text = item.SubjectName
        //        });
        //    }
        //    foreach (var item in fieldsOfStudies)
        //    {
        //        viewModelCreate.FieldsOfStudies.Add(new SelectListItem()
        //        {
        //            Value = item.FieldOfStudiesID.ToString(),
        //            Text = item.FieldOfStudiesName
        //        });
        //    }

        //    return viewModelCreate;
        //}

        /// <summary>
        /// Befüllt die Properties einer neuen Frage aus dem ViewModelCreate Objekt und speichert die Frage in der DB
        /// </summary>
        /// <param name="viewModelCreate"></param>
        private void CreateNewQuestion(ViewModelCreate viewModelCreate)
        {
            DtQuestion question = new DtQuestion();

            question.Title = viewModelCreate.QuestionTitle;
            question.Content = viewModelCreate.QuestionContent;

            question.FieldOfStudiesID = viewModelCreate.FieldOfStudiesId;
            question.SubjectID = viewModelCreate.SubjectId;
            question.SubmissionDate = DateTime.Now;
            question.UniversityID = viewModelCreate.UniversityId;

            // TODO Benjamin: check ob alle Angaben richtig sind

            _context.Add(question);

            _context.SaveChanges();
        }



        #endregion

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

        public IActionResult Done(ViewModelSearch viewModelSearch)
        {
            return View(viewModelSearch);
        }

        public IActionResult CreateNewEntry()
        {
            return View();
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
