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
using XamPass.Models.ViewModels;
using XamPass.Models.DataBaseModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;

namespace XamPass.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        private ILogger _logger;
        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        #region Controller

        /// <summary>
        /// Standard Constructor
        /// Gets the Logger and the Database Connection via Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public HomeController(DataContext context, ILogger<HomeController> logger, IStringLocalizer<HomeController> stringLocalizer)
        {
            _context = context;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        #endregion

        #region Homepage

        /// <summary>
        /// Is called when the Index Page is loaded
        /// Loads all Data for the Filter Dropdowns from the Database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            ViewModelSearch viewModelSearch = new ViewModelSearch();

            // Fill the Dropdowns with all the Data from the Db
            viewModelSearch.FillAllDropdowns(_context);

            return View(viewModelSearch);
        }

        /// <summary>
        /// Fills all the Filter Dropdowns with the Data from the Database
        /// Filters the Dropdowns if another Filter was set
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ViewModelSearch viewModelSearch)
        {
            // Fill the Dropdowns with all the Data from the Db
            viewModelSearch.FillAllDropdowns(_context);

            // Set Filters for the Dropdown Lists
            SetAllFilters(viewModelSearch);

            // questions should NOT be rendered
            viewModelSearch.SearchExecuted = false;

            viewModelSearch.FilterUniversitiesByFederalState(_context);
            
            return View(viewModelSearch);
        }

        /// <summary>
        /// Gets called when in the Main View a selection was made and the Search Button has been hit.
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowQuestions(ViewModelSearch viewModelSearch)
        {
            try
            {
                // questions should be rendered
                viewModelSearch.SearchExecuted = true;

                // get entries from db
                var fieldsOfStudies = _context.FieldsOfStudies.OrderBy(f => f.FieldOfStudiesName).ToListAsync().Result;
                var subjects = _context.Subjects.OrderBy(s => s.SubjectName).ToListAsync().Result;
                var federalStates = _context.FederalStates.OrderBy(f => f.FederalStateName).ToListAsync().Result;
                var universities = _context.Universities.OrderBy(u => u.UniversityName).ToListAsync().Result;

                foreach (var item in fieldsOfStudies)
                {
                    viewModelSearch.FieldsOfStudies.Add(new SelectListItem
                    {
                        Value = item.FieldOfStudiesID.ToString(),
                        Text = item.FieldOfStudiesName
                    });
                }
                foreach (var item in subjects)
                {
                    viewModelSearch.Subjects.Add(new SelectListItem
                    {
                        Value = item.SubjectID.ToString(),
                        Text = item.SubjectName
                    });
                }
                foreach (var item in federalStates)
                {
                    viewModelSearch.FederalStates.Add(new SelectListItem
                    {
                        Value = item.FederalStateID.ToString(),
                        Text = item.FederalStateName
                    });
                }
                foreach (var item in universities)
                {
                    viewModelSearch.Universities.Add(new SelectListItem
                    {
                        Value = item.UniversityID.ToString(),
                        Text = item.UniversityName
                    });
                }

                //Build the filter and load the Questions from the Database
                List<DtQuestion> filteredQuestions = _context.Questions
                    .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
                    .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
                    .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
                    .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
                    .ToList();

                //viewModelQuestions = GetViewModelQuestions(viewModelQuestions, true).Result;
                viewModelSearch.Questions = filteredQuestions.OrderByDescending(q => q.UpVotes).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading filtered Questions from Db");
            }

            return View("Index", viewModelSearch);
        }

        #endregion

        #region View Question

        /// <summary>
        /// Gets called when a single Question is selected and the Details of that Question have to be loaded
        /// Loads the Details of the Question to the viewModelQuestions and returns the Details View
        /// </summary>
        /// <param name="viewModelQuestion"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ViewQuestion(int? id)
        {
            ViewModelQuestion viewModelQuestion = new ViewModelQuestion();
            viewModelQuestion.QuestionId = (int?)id;

            try
            {
                // Loads the selected Question from the Database
                viewModelQuestion.Question = _context.Questions
                    .Include(q => q.FieldOfStudies)
                    .Include(q => q.Subject)
                    .Include(q => q.University)
                    .ThenInclude(u => u.FederalState)
                    .Include(u => u.University.Country)
                    .Include(q => q.Answers)
                    .SingleOrDefault(q => q.QuestionID == viewModelQuestion.QuestionId);


                // Fill the Properties for the View
                if (viewModelQuestion.Question != null)
                {
                    viewModelQuestion.FieldOfStudies = viewModelQuestion.Question.FieldOfStudies;
                    viewModelQuestion.Subject = viewModelQuestion.Question.Subject;
                    viewModelQuestion.University = viewModelQuestion.Question.University;
                    viewModelQuestion.Country = viewModelQuestion.Question.University.Country;
                    viewModelQuestion.FederalState = viewModelQuestion.Question.University.FederalState;
                    viewModelQuestion.Answers = viewModelQuestion.Question.Answers;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading the selected Question from the Db");
            }
            return View(viewModelQuestion);
        }

        /// <summary>
        /// Gets called, when a new Answer is created
        /// Loads the responding Question from the Database and adds the Answer
        /// </summary>
        /// <param name="viewModelQuestions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewQuestion(ViewModelQuestion viewModelQuestions)
        {
            try
            {
                // Loads the selected Question from the Database
                viewModelQuestions.Question = _context.Questions
                    .Include(q => q.FieldOfStudies)
                    .Include(q => q.Subject)
                    .Include(q => q.University)
                    .ThenInclude(u => u.FederalState)
                    .Include(u => u.University.Country)
                    .Include(q => q.Answers)
                    .SingleOrDefault(q => q.QuestionID == viewModelQuestions.QuestionId);


                // Fill the Properties for the View
                if (viewModelQuestions.Question != null)
                {
                    viewModelQuestions.FieldOfStudies = viewModelQuestions.Question.FieldOfStudies;
                    viewModelQuestions.Subject = viewModelQuestions.Question.Subject;
                    viewModelQuestions.University = viewModelQuestions.Question.University;
                    viewModelQuestions.Country = viewModelQuestions.Question.University.Country;
                    viewModelQuestions.FederalState = viewModelQuestions.Question.University.FederalState;
                    viewModelQuestions.Answers = viewModelQuestions.Question.Answers;
                }

                if (viewModelQuestions.Answer != null)
                {
                    // Adding new Answer and save it to Database
                    viewModelQuestions.Answer.SubmissionDate = DateTime.Now;
                    viewModelQuestions.Question.Answers.Add(viewModelQuestions.Answer);

                    _context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding new Answer to a question and storing it to the Db");
            }

            return View(viewModelQuestions);
        }

        #endregion

        #region Create Question

        /// <summary>
        /// Gets called when a new Question should be added to the Database
        /// Fills all the Properties and Dropdowns with the Values from the Database
        /// </summary>
        /// <param name="viewModelCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateQuestion(ViewModelCreate viewModelCreate)
        {
            // get entries from db
            viewModelCreate.FillAllDropdowns(_context);           

            viewModelCreate.FilterUniversitiesByFederalState(_context);
            return View(viewModelCreate);
        }

        /// <summary>
        /// Creates new DtQuestion Object with the Properties from the View and Saves it to the Database
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewEntry(ViewModelCreate viewModelCreate)
        {
            // If all entries are correct
            if (ModelState.IsValid)
            {
                DtQuestion question = new DtQuestion();

                if (viewModelCreate.QuestionTitle != null)
                {
                    question.Title = viewModelCreate.QuestionTitle;

                }
                else
                {
                    question.Title = "Neue Frage";

                }
                question.Content = viewModelCreate.QuestionContent;

                if (viewModelCreate.AnswerContent != null)
                {
                    question.Answers.Add(new DtAnswer()
                    {
                        Content = viewModelCreate.AnswerContent,
                        SubmissionDate = DateTime.Now,
                        UpVotes = 3
                    });
                }

                question.FieldOfStudiesID = (int)viewModelCreate.FieldOfStudiesId;
                question.SubjectID = (int)viewModelCreate.SubjectId;
                question.SubmissionDate = DateTime.Now;
                question.UniversityID = (int)viewModelCreate.UniversityId;

                try
                {
                    _context.Add(question);

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while adding and saving new Question to the database");
                    return View("CreateQuestion", viewModelCreate);
                }

                return RedirectToAction("ViewQuestion", new { id = question.QuestionID });
            }

            // Todo hier das viewModelCreate wieder befüllen

            // if not all entries are correct you are redirected
            viewModelCreate.FillAllDropdowns(_context);
            return View("CreateQuestion", viewModelCreate);
        }
        #endregion

        #region GetViewModels

        ///// <summary>
        ///// Fills the Properties for the Dropdowns in the View to Create a new Questions with data from DB
        ///// </summary>
        ///// <param name="viewModelCreate"></param>
        ///// <returns></returns>
        //private async Task<ViewModelCreate> GetViewModelCreate(ViewModelCreate viewModelCreate)
        //{
        //    try
        //    {
        //        var universities = await _context.Universities.ToListAsync();
        //        var federalStates = await _context.FederalStates.ToListAsync();
        //        var subjects = await _context.Subjects.ToListAsync();
        //        var fieldsOfStudies = await _context.FieldsOfStudies.ToListAsync();

        //        foreach (var item in universities)
        //        {
        //            viewModelCreate.Universities.Add(new SelectListItem()
        //            {
        //                Value = item.UniversityID.ToString(),
        //                Text = item.UniversityName
        //            });
        //        }
        //        foreach (var item in federalStates)
        //        {
        //            viewModelCreate.FederalStates.Add(new SelectListItem()
        //            {
        //                Value = item.FederalStateID.ToString(),
        //                Text = item.FederalStateName
        //            });
        //        }
        //        foreach (var item in subjects)
        //        {
        //            viewModelCreate.Subjects.Add(new SelectListItem()
        //            {
        //                Value = item.SubjectID.ToString(),
        //                Text = item.SubjectName
        //            });
        //        }
        //        foreach (var item in fieldsOfStudies)
        //        {
        //            viewModelCreate.FieldsOfStudies.Add(new SelectListItem()
        //            {
        //                Value = item.FieldOfStudiesID.ToString(),
        //                Text = item.FieldOfStudiesName
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error while loading Data for the Dropdowns from Db");
        //    }

        //    return viewModelCreate;
        //}
        #endregion

        #region Temporary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Done(ViewModelSearch viewModelSearch)
        {
            // TODO irgendwann muss das noch weg
            var result = viewModelSearch;
            return View(result);
        }
        #endregion

        #region Filter Dropdowns

        /// <summary>
        /// Gets all Questions from the Database for the applied Filter hands them to the 3 Filter Functions
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        public void SetAllFilters(ViewModelSearch viewModelSearch)
        {
            List<DtQuestion> questions = null;

            try
            {
                // Get all Questions for the applied Filter
                questions = _context.Questions
                   .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
                   .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
                   .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
                   .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
                   .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading filtered Questions from the Database");
            }
            // if no filter is set, the Dropdowns don´t have to be filtered
            if (viewModelSearch.SubjectId.HasValue || viewModelSearch.UniversityId.HasValue || viewModelSearch.FieldOfStudiesId.HasValue)
            {
                SetFilterForUniversities(questions, viewModelSearch);
                SetFilterForFieldsOfStudies(questions, viewModelSearch);
                SetFilterForSubjects(questions, viewModelSearch);
            }
        }

        /// <summary>
        /// Filters the University-Dropdown
        /// Gets all Universities from the filtered Questions and Adds them to the Dropdown
        /// </summary>
        /// <param name="questions"></param>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        public void SetFilterForUniversities(List<DtQuestion> questions, ViewModelSearch viewModelSearch)
        {
            //List<DtUniversity> universities = new List<DtUniversity>();            

            var universities = questions
               .Select(q => q.University)
               .Distinct()
               .OrderBy(u => u.UniversityName)
               .ToList();

            // if no filter was set or no element was returned all Elements stay in the List
            if (universities.Count == 0)
            {
                // TODO: try-catch
                universities = _context.Universities.OrderBy(u => u.UniversityName).ToList();
            }

            viewModelSearch.Universities.Clear();

            foreach (var item in universities)
            {
                viewModelSearch.Universities.Add(
                    new SelectListItem { Value = item.UniversityID.ToString(), Text = item.UniversityName });
            }
        }

        /// <summary>
        /// Filters the FieldsOfStudies-Dropdown
        /// Gets all FieldsOfStudies from the filtered Questions and Adds them to the Dropdown
        /// </summary>
        /// <param name="questions"></param>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        public void SetFilterForFieldsOfStudies(List<DtQuestion> questions, ViewModelSearch viewModelSearch)
        {
            //List<DtFieldOfStudies> fieldsOfStudies = new List<DtFieldOfStudies>();

            var fieldsOfStudies = questions
               .Select(q => q.FieldOfStudies)
               .Distinct()
               .OrderBy(fos => fos.FieldOfStudiesName)
               .ToList();

            // if no filter was set or no element was returned all Elements stay in the List
            if (fieldsOfStudies.Count == 0)
            {
                // TODO: try-catch
                fieldsOfStudies = _context.FieldsOfStudies.OrderBy(f => f.FieldOfStudiesName).ToList();
            }

            viewModelSearch.FieldsOfStudies.Clear();

            foreach (var item in fieldsOfStudies)
            {
                viewModelSearch.FieldsOfStudies.Add(
                   new SelectListItem { Value = item.FieldOfStudiesID.ToString(), Text = item.FieldOfStudiesName });
            }
        }

        /// <summary>
        /// Filters the FieldsOfStudies-Dropdown
        /// Gets all Subjects from the filtered Questions and Adds them to the Dropdown
        /// </summary>
        /// <param name="questions"></param>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        public void SetFilterForSubjects(List<DtQuestion> questions, ViewModelSearch viewModelSearch)
        {
            var subjects = questions
               .Select(q => q.Subject)
                .Distinct()
                .OrderBy(s => s.SubjectName)
                .ToList();

            // if no filter was set or no element was returned all Elements stay in the List
            if (subjects.Count == 0)
            {
                // TODO: try-catch
                subjects = _context.Subjects.OrderBy(s => s.SubjectName).ToList();
            }

            viewModelSearch.Subjects.Clear();

            foreach (var item in subjects)
            {
                viewModelSearch.Subjects.Add(
                    new SelectListItem { Value = item.SubjectID.ToString(), Text = item.SubjectName });
            }
        }

        #endregion

        #region Error
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region new Field Of Studies, Subject, University

        /// <summary>
        /// Add a new FieldofStudies Entity
        /// </summary>
        /// <param name="viewModelCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewFieldOfStudies(ViewModelCreate viewModelCreate)
        {
            ViewModelCreateFieldOfStudies vmFieldOfStudies = new ViewModelCreateFieldOfStudies();

            return View(vmFieldOfStudies);
        }

        /// <summary>
        /// Saves new Field of Studies Entity to the Database
        /// </summary>
        /// <param name="vmFieldOfStudies"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewFieldOfStudies(ViewModelCreateFieldOfStudies vmFieldOfStudies)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DtFieldOfStudies fieldOfStudies = new DtFieldOfStudies();
                    fieldOfStudies.FieldOfStudiesName = vmFieldOfStudies.FieldOfStudiesName;

                    _context.Add(fieldOfStudies);
                    await _context.SaveChangesAsync();

                    ViewModelCreate viewModelCreate = new ViewModelCreate();
                    //viewModelCreate = GetViewModelCreate(viewModelCreate).Result;
                    viewModelCreate.FillAllDropdowns(_context);
                    viewModelCreate.FieldOfStudiesId = fieldOfStudies.FieldOfStudiesID;

                    return View("CreateQuestion", viewModelCreate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving new Field of Studies to the Database");
            }
            return View("CreateFieldOfStudies", vmFieldOfStudies);
        }

        /// <summary>
        /// Create new Subject Entity
        /// </summary>
        /// <param name="viewModelCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewSubject(ViewModelCreate viewModelCreate)
        {
            ViewModelCreateSubject vmSubject = new ViewModelCreateSubject();

            return View(vmSubject);
        }

        /// <summary>
        /// Saves new Subject to the Databse
        /// </summary>
        /// <param name="vmSubject"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewSubject(ViewModelCreateSubject vmSubject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DtSubject subject = new DtSubject();
                    subject.SubjectName = vmSubject.SubjectName;

                    _context.Add(subject);
                    await _context.SaveChangesAsync();

                    ViewModelCreate viewModelCreate = new ViewModelCreate();
                    //viewModelCreate = GetViewModelCreate(viewModelCreate).Result;
                    viewModelCreate.FillAllDropdowns(_context);
                    viewModelCreate.SubjectId = subject.SubjectID;

                    return View("CreateQuestion", viewModelCreate);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving new Subject to the Db");
            }

            return View("CreateSubject", vmSubject);
        }

        /// <summary>
        /// Create new University Entity
        /// </summary>
        /// <param name="viewModelCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewUniversity(ViewModelCreate viewModelCreate)
        {
            ViewModelCreateUniversity vmUniversity = new ViewModelCreateUniversity();

            var federalStates = await _context.FederalStates.ToListAsync();

            foreach (var item in federalStates)
            {
                vmUniversity.FederalStates.Add(new SelectListItem()
                {
                    Value = item.FederalStateID.ToString(),
                    Text = item.FederalStateName
                });
            }

            return View(vmUniversity);
        }

        /// <summary>
        /// Save new Uniersity to the Db
        /// </summary>
        /// <param name="vmUniversity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewUniversity(ViewModelCreateUniversity vmUniversity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DtUniversity university = new DtUniversity();

                    university.CountryID = 1;
                    university.FederalStateID = (int)vmUniversity.FederalStateId;
                    university.UniversityName = vmUniversity.UniversityName;

                    _context.Add(university);
                    await _context.SaveChangesAsync();

                    ViewModelCreate viewModelCreate = new ViewModelCreate();
                    //viewModelCreate = GetViewModelCreate(viewModelCreate).Result;
                    viewModelCreate.FillAllDropdowns(_context);
                    viewModelCreate.UniversityId = university.UniversityID;

                    return View("CreateQuestion", viewModelCreate);
                }

                var federalStates = await _context.FederalStates.ToListAsync();

                foreach (var item in federalStates)
                {
                    vmUniversity.FederalStates.Add(new SelectListItem()
                    {
                        Value = item.FederalStateID.ToString(),
                        Text = item.FederalStateName
                    });
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving new University to the Database");
            }

            return View("CreateUniversity", vmUniversity);
        }

        /// <summary>
        /// Redirects to the Create Question View if the Creation of a new Entity was canceled
        /// </summary>
        /// <returns></returns>
        public IActionResult CancelNewField()
        {
            ViewModelCreate viewModelCreate = new ViewModelCreate();
            //viewModelCreate = GetViewModelCreate(viewModelCreate).Result;
            viewModelCreate.FillAllDropdowns(_context);

            return View("CreateQuestion", viewModelCreate);
        }

        #endregion
    }
}
