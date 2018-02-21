using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XamPass.Models;
using XamPass.Models.DataBaseModels;

namespace XamPass.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        #region Homepage

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                ViewModelSearch viewModelSearch = new ViewModelSearch();
                viewModelSearch = GetViewModelSearch(viewModelSearch).Result;
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
            viewModelSearch = GetViewModelSearch(viewModelSearch).Result;

            // setzt gewählte Hochschule auf null, wenn gewähltes Bundesland nicht übereinstimmt
            if (viewModelSearch.UniversityId.HasValue)
            {
                var university = viewModelSearch.Universities.FirstOrDefault(u => u.UniversityID == viewModelSearch.UniversityId);
                if (university.FederalStateID != viewModelSearch.FederalStateId)
                {
                    viewModelSearch.UniversityId = null;
                }
            }
            // filtert Hochschulen für gewähltes Bundesland
            if (viewModelSearch.FederalStateId.HasValue)
            {
                viewModelSearch.UniversitySelectList = new List<SelectListItem>();
                foreach (var item in viewModelSearch.Universities)
                {
                    if (item.FederalStateID == viewModelSearch.FederalStateId)
                    {
                        viewModelSearch.UniversitySelectList.Add(
                            new SelectListItem { Value = item.UniversityID.ToString(), Text = item.UniversityName });
                    }
                }
            }

            return View(viewModelSearch);
        }
        #endregion

        #region ShowQuestions
        public IActionResult ShowQuestions(ViewModelSearch viewModelSearch)
        {
            viewModelSearch = GetViewModelSearch(viewModelSearch).Result;

            //var questions = new List<DtQuestion>();

            // Alle Fragen werden aus der Datenbank geladen und danach mit den eingegebenen Filtern durchsucht
            //questions = _context.Questions.ToList();
            ViewModelQuestions viewModelQuestions = new ViewModelQuestions();
            viewModelQuestions = GetViewModelQuestions(viewModelQuestions).Result;

            if (viewModelSearch.FieldOfStudiesId.HasValue)
            {
                viewModelQuestions.Questions = viewModelQuestions.Questions.Where(q => q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId).ToList();
            }

            if (viewModelSearch.SubjectId.HasValue)
            {
                viewModelQuestions.Questions = viewModelQuestions.Questions.Where(q => q.SubjectID == viewModelSearch.SubjectId).ToList();
            }

            if (viewModelSearch.FederalStateId.HasValue)
            {
                viewModelQuestions.Questions = viewModelQuestions.Questions.Where(q => q.University.FederalStateID == viewModelSearch.FederalStateId).ToList();
            }

            if (viewModelSearch.UniversityId.HasValue)
            {
                viewModelQuestions.Questions = viewModelQuestions.Questions.Where(q => q.UniversityID == viewModelSearch.UniversityId).ToList();
            }

            return View(viewModelQuestions);
        }
        #endregion

        /// <summary>
        /// Filters Universities by Federal State
        /// </summary>
        /// <param name="viewModelSearch">Data From the View</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateQuestion(ViewModelSearch viewModelSearch)
        {
            viewModelSearch = GetViewModelSearch(viewModelSearch).Result;

            // setzt gewählte Hochschule auf 0, wenn gewähltes Bundesland nicht übereinstimmt
            if (viewModelSearch.UniversityId.HasValue)
            {
                var university = viewModelSearch.Universities.FirstOrDefault(u => u.UniversityID == viewModelSearch.UniversityId);
                if (university.FederalStateID != viewModelSearch.FederalStateId)
                {
                    viewModelSearch.UniversityId = null;
                }
            }
            // filtert Hochschulen für gewähltes Bundesland
            if (viewModelSearch.FederalStateId.HasValue)
            {
                viewModelSearch.UniversitySelectList = new List<SelectListItem>();
                foreach (var item in viewModelSearch.Universities)
                {
                    if (item.FederalStateID == viewModelSearch.FederalStateId)
                    {
                        viewModelSearch.UniversitySelectList.Add(
                            new SelectListItem { Value = item.UniversityID.ToString(), Text = item.UniversityName });
                    }
                }
            }

            //return RedirectToAction("Done", result);

            return View(viewModelSearch);
            //return RedirectToAction("CreateNewEntry", viewModelCreate);

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

        ///// <summary>
        ///// Befüllt die Properties einer neuen Frage aus dem ViewModelCreate Objekt und speichert die Frage in der DB
        ///// </summary>
        ///// <param name="viewModelCreate"></param>
        //private void CreateNewQuestion(ViewModelCreate viewModelCreate)
        //{
        //    var result = viewModelCreate;

        //    DtQuestion question = new DtQuestion();

        //    question.Title = viewModelCreate.QuestionTitle;
        //    question.Content = viewModelCreate.QuestionContent;

        //    question.FieldOfStudiesID = viewModelCreate.FieldOfStudiesId;
        //    question.SubjectID = viewModelCreate.SubjectId;
        //    question.SubmissionDate = DateTime.Now;
        //    question.UniversityID = viewModelCreate.UniversityId;

        //    // TODO Benjamin: check ob alle Angaben richtig sind

        //    _context.Add(question);

        //    _context.SaveChanges();
        //}



        //#endregion

        #region View Question

        [HttpGet]
        public IActionResult ViewQuestion(ViewModelQuestions viewModelQuestions)
        {
            viewModelQuestions = GetViewModelQuestions(viewModelQuestions).Result;

            var fieldsOfStudies = _context.FieldsOfStudies.ToList();
            var subjects = _context.Subjects.ToList();
            var universities = _context.Universities.ToList();
            var countries = _context.Countries.ToList();
            var federalStates = _context.FederalStates.ToList();
            var answers = _context.Answers.ToList();

            viewModelQuestions.Question = viewModelQuestions.Questions.FirstOrDefault(q => q.QuestionID == viewModelQuestions.QuestionId);

            viewModelQuestions.FieldOfStudies = fieldsOfStudies.FirstOrDefault(f => f.FieldOfStudiesID == viewModelQuestions.Question.FieldOfStudiesID);
            viewModelQuestions.Subject = subjects.FirstOrDefault(f => f.SubjectID == viewModelQuestions.Question.SubjectID);
            viewModelQuestions.University = universities.FirstOrDefault(f => f.UniversityID == viewModelQuestions.Question.UniversityID);
            viewModelQuestions.Country = countries.FirstOrDefault(f => f.CountryID == viewModelQuestions.Question.University.CountryID);
            viewModelQuestions.FederalState = federalStates.FirstOrDefault(f => f.FederalStateID == viewModelQuestions.Question.University.FederalStateID);
            viewModelQuestions.Answers = answers.Where(a => a.QuestionId == viewModelQuestions.QuestionId).ToList();

            return View(viewModelQuestions);
        }

        #endregion

        #region Authorization
        //[Authorize]
        //public IActionResult CreateDB()
        //{       
        //    DBInitialize.DatabaseTest(_context);
        //    return RedirectToAction("Done");
        //}

        //public IActionResult Login()

        //{

        //    if (HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Admin");
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginInputModel inputModel)
        //{
        //    if (!(inputModel.Username == "admin" && inputModel.Password == "password"))
        //        return View();

        //    // create claims
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, "admin")
        //        //new Claim(ClaimTypes.Email, inputModel.Username)
        //    };

        //    // create identity
        //    ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");

        //    // create principal
        //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        //    // sign-in
        //    await HttpContext.SignInAsync(
        //            scheme: "AdminCookieScheme",
        //            principal: principal);

        //    return RedirectToAction("Index", "Admin");
        //}

        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(
        //            scheme: "AdminCookieScheme");

        //    return RedirectToAction("Login");
        //}
        #endregion

        /// <summary>
        /// Creates new DtQuestion Object with the Properties from the View and Saves it to the Database
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewEntry(ViewModelSearch viewModelSearch)
        {
            viewModelSearch = GetViewModelSearch(viewModelSearch).Result;

            // If all entries are correct
            if (ModelState.IsValid)
            {
                DtQuestion question = new DtQuestion();

                //question.Title = viewModelSearch.QuestionTitle;
                question.Title = "Neue Frage";
                question.Content = viewModelSearch.QuestionContent;

                question.FieldOfStudiesID = (int)viewModelSearch.FieldOfStudiesId;
                question.SubjectID = (int)viewModelSearch.SubjectId;
                question.SubmissionDate = DateTime.Now;
                question.UniversityID = (int)viewModelSearch.UniversityId;

                _context.Add(question);

                _context.SaveChanges();

                return RedirectToAction("Done");
            }

            // if not all entries are correct you are redirected
            return View("CreateQuestion", viewModelSearch);
        }

        #region GetViewModels
        private async Task<ViewModelSearch> GetViewModelSearch(ViewModelSearch viewModelSearch)
        {
            var universities = await _context.Universities.ToListAsync();
            var federalStates = await _context.FederalStates.ToListAsync();
            var subjects = await _context.Subjects.ToListAsync();
            var fieldsOfStudies = await _context.FieldsOfStudies.ToListAsync();

            //var viewModelSearch = new ViewModelSearch();
            viewModelSearch.Universities = universities;

            foreach (var item in universities)
            {
                viewModelSearch.UniversitySelectList.Add(new SelectListItem()
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

        private async Task<ViewModelQuestions> GetViewModelQuestions(ViewModelQuestions viewModelQuestions)
        {
            var questions = await _context.Questions.ToListAsync();

            viewModelQuestions.Questions = questions;

            foreach (var item in questions)
            {
                viewModelQuestions.QuestionsSelectList.Add(new SelectListItem()
                {
                    Value = item.QuestionID.ToString(),
                    Text = item.Content
                });
            }
            return viewModelQuestions;
        }
        #endregion

        public IActionResult Done(ViewModelSearch viewModelSearch)
        {
            var result = viewModelSearch;
            return View(result);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
