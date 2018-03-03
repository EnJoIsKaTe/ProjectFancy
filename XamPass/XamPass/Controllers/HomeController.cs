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

namespace XamPass.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        private List<DtFieldOfStudies> _fieldsOfStudies;
        private List<DtSubject> _subjects;
        private List<DtFederalState> _federalStates;
        private List<DtUniversity> _universities;


        #region Controller
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;

            _fieldsOfStudies = new List<DtFieldOfStudies>();
            _subjects = new List<DtSubject>();
            _federalStates = new List<DtFederalState>();
            _universities = new List<DtUniversity>();
        }
        #endregion

        #region Homepage

        [HttpGet]
        public IActionResult Index()
        {
            // TODO: proper error handling, this one is for development only
            try
            {
                ViewModelSearch viewModelSearch = new ViewModelSearch();
                //viewModelSearch = GetViewModelSearch(viewModelSearch).Result;

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
                return View(viewModelSearch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("CreateDB", "Admin");
            }
        }

        [HttpPost]
        public IActionResult Index(ViewModelSearch viewModelSearch)
        {
            // Fill the Dropdowns with all the Data from the Db
            viewModelSearch = FillAllDropdowns(viewModelSearch);

            var universities = _context.Universities.OrderBy(u => u.UniversityName).ToListAsync().Result;

            // questions should NOT be rendered
            viewModelSearch.SearchExecuted = false;

            // sets selected University to null if the seleected FederalState does not fit
            if (viewModelSearch.UniversityId.HasValue)
            {
                //var university = viewModelSearch.Universities.FirstOrDefault(u => u.UniversityID == viewModelSearch.UniversityId);
                var university = universities.FirstOrDefault(u => u.UniversityID == viewModelSearch.UniversityId);
                if (university.FederalStateID != viewModelSearch.FederalStateId)
                {
                    viewModelSearch.UniversityId = null;
                }
            }
            // filter Universities by selected FederalState
            if (viewModelSearch.FederalStateId.HasValue)
            {
                viewModelSearch.Universities = new List<SelectListItem>();
                foreach (var item in universities)
                {
                    if (item.FederalStateID == viewModelSearch.FederalStateId)
                    {
                        viewModelSearch.Universities.Add(
                            new SelectListItem { Value = item.UniversityID.ToString(), Text = item.UniversityName });
                    }
                }
            }

            // Testarea
            SetAllFilters(viewModelSearch);
            

            return View(viewModelSearch);
        }


        /// <summary>
        /// Fills the ViewModelSearch Dropdowns with the Data from the Database
        /// Dropdowns will be sorted alphabetically
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        public ViewModelSearch FillAllDropdowns(ViewModelSearch viewModelSearch)
        {
            // get entries from db            
            _fieldsOfStudies = _context.FieldsOfStudies.OrderBy(f => f.FieldOfStudiesName).ToList();
            _subjects = _context.Subjects.OrderBy(s => s.SubjectName).ToList();
            _federalStates = _context.FederalStates.OrderBy(f => f.FederalStateName).ToList();
            _universities = _context.Universities.OrderBy(u => u.UniversityName).ToList();

            //viewModelSearch.Universities = universities;
            foreach (var item in _fieldsOfStudies)
            {
                viewModelSearch.FieldsOfStudies.Add(new SelectListItem
                {
                    Value = item.FieldOfStudiesID.ToString(),
                    Text = item.FieldOfStudiesName
                });
            }
            foreach (var item in _subjects)
            {
                viewModelSearch.Subjects.Add(new SelectListItem
                {
                    Value = item.SubjectID.ToString(),
                    Text = item.SubjectName
                });
            }
            foreach (var item in _federalStates)
            {
                viewModelSearch.FederalStates.Add(new SelectListItem
                {
                    Value = item.FederalStateID.ToString(),
                    Text = item.FederalStateName
                });
            }
            foreach (var item in _universities)
            {
                viewModelSearch.Universities.Add(new SelectListItem
                {
                    Value = item.UniversityID.ToString(),
                    Text = item.UniversityName
                });
            }

            return viewModelSearch;
        }


        public ViewModelSearch SetAllFilters(ViewModelSearch viewModelSearch)
        {
            List<DtQuestion> questions = _context.Questions
               .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
               .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
               .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
               .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
               .ToList();

            // if no filter is set the Dropdowns don´t have to be filtered
            if (viewModelSearch.SubjectId.HasValue || viewModelSearch.UniversityId.HasValue || viewModelSearch.FieldOfStudiesId.HasValue)
            {
                viewModelSearch = SetFilterForUniversities(questions, viewModelSearch);
                viewModelSearch = SetFilterForFieldsOfStudies(questions, viewModelSearch);
                viewModelSearch = SetFilterForSubjects(questions, viewModelSearch);
            }

            return viewModelSearch;
        }


        public ViewModelSearch SetFilterForUniversities(List<DtQuestion> questions, ViewModelSearch viewModelSearch)
        {
            List<DtUniversity> universities = new List<DtUniversity>();
            
            universities = questions
               .Select(q => q.University)
               .Distinct()
               .OrderBy(u => u.UniversityName)
               .ToList();


            // if no filter was set or no element was returned all Elements stay in the List
            if (universities.Count == 0)
                universities.AddRange(_universities);

            viewModelSearch.Universities.Clear();

            foreach (var item in universities)
            {
                viewModelSearch.Universities.Add(
                    new SelectListItem { Value = item.UniversityID.ToString(), Text = item.UniversityName });
            }

            return viewModelSearch;
        }

        public ViewModelSearch SetFilterForFieldsOfStudies(List<DtQuestion> questions, ViewModelSearch viewModelSearch)
        {
            List<DtFieldOfStudies> fieldsOfStudies = new List<DtFieldOfStudies>();

            fieldsOfStudies = questions
               .Select(q => q.FieldOfStudies)
               .Distinct()
               .OrderBy(fos => fos.FieldOfStudiesName)
               .ToList();

            // if no filter was set or no element was returned all Elements stay in the List
            if (fieldsOfStudies.Count == 0)
                fieldsOfStudies.AddRange(_fieldsOfStudies);

            viewModelSearch.FieldsOfStudies.Clear();

            foreach (var item in fieldsOfStudies)
            {
                viewModelSearch.FieldsOfStudies.Add(
                   new SelectListItem { Value = item.FieldOfStudiesID.ToString(), Text = item.FieldOfStudiesName });
            }

            return viewModelSearch;
        }

        public ViewModelSearch SetFilterForSubjects(List<DtQuestion> questions, ViewModelSearch viewModelSearch)
        {
            List<DtSubject> subjects = new List<DtSubject>();

            subjects = questions
               .Select(q => q.Subject)
                .Distinct()
                .OrderBy(s => s.SubjectName)
                .ToList();

            // if no filter was set or no element was returned all Elements stay in the List
            if (subjects.Count == 0)
                subjects.AddRange(_subjects);

            viewModelSearch.Subjects.Clear();

            foreach (var item in subjects)
            {
                viewModelSearch.Subjects.Add(
                    new SelectListItem { Value = item.SubjectID.ToString(), Text = item.SubjectName });
            }

            return viewModelSearch;
        }

        /// <summary>
        /// Gets called when in the Main View a selection was made and the Search Button has been hit.
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        public IActionResult ShowQuestions(ViewModelSearch viewModelSearch)
        {
            // questions should be rendered
            viewModelSearch.SearchExecuted = true;

            //viewModelSearch = GetViewModelSearch(viewModelSearch).Result;

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
            viewModelSearch.Questions = filteredQuestions;

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

            //viewModelQuestion = GetViewModelQuestions(viewModelQuestion, false).Result;

            //viewModelQuestion.Question = viewModelQuestion.Questions.FirstOrDefault(q => q.QuestionID == viewModelQuestion.QuestionId);

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
            return View(viewModelQuestion);
        }

        /// <summary>
        /// Gets called, when a new Answer is created
        /// Loads the responding Question from the Database and adds the Answer
        /// </summary>
        /// <param name="viewModelQuestions"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ViewQuestion(ViewModelQuestion viewModelQuestions)
        {
            //viewModelQuestions = GetViewModelQuestions(viewModelQuestions, false).Result;

            //viewModelQuestions.Question = viewModelQuestions.Questions.FirstOrDefault(q => q.QuestionID == viewModelQuestions.QuestionId);

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

            return View(viewModelQuestions);
        }
        #endregion

        #region Create Question
        [HttpPost]
        public IActionResult CreateQuestion(ViewModelCreate viewModelCreate)
        {
            //viewModelCreate = GetViewModelCreate(viewModelCreate).Result;

            // get entries from db
            var fieldsOfStudies = _context.FieldsOfStudies.OrderBy(f => f.FieldOfStudiesName).ToListAsync().Result;
            var subjects = _context.Subjects.OrderBy(s => s.SubjectName).ToListAsync().Result;
            var federalStates = _context.FederalStates.OrderBy(f => f.FederalStateName).ToListAsync().Result;
            var universities = _context.Universities.OrderBy(u => u.UniversityName).ToListAsync().Result;

            foreach (var item in fieldsOfStudies)
            {
                viewModelCreate.FieldsOfStudies.Add(new SelectListItem
                {
                    Value = item.FieldOfStudiesID.ToString(),
                    Text = item.FieldOfStudiesName
                });
            }
            foreach (var item in subjects)
            {
                viewModelCreate.Subjects.Add(new SelectListItem
                {
                    Value = item.SubjectID.ToString(),
                    Text = item.SubjectName
                });
            }
            foreach (var item in federalStates)
            {
                viewModelCreate.FederalStates.Add(new SelectListItem
                {
                    Value = item.FederalStateID.ToString(),
                    Text = item.FederalStateName
                });
            }
            foreach (var item in universities)
            {
                viewModelCreate.Universities.Add(new SelectListItem
                {
                    Value = item.UniversityID.ToString(),
                    Text = item.UniversityName
                });
            }

            // if chosen university doesn't match the federal state
            // set federal state to null
            if (viewModelCreate.UniversityId.HasValue)
            {
                //var university = _context.Universities.FirstOrDefault(u => u.UniversityID == viewModelCreate.UniversityId);
                var university = universities.FirstOrDefault(u => u.UniversityID == viewModelCreate.UniversityId);
                if (university.FederalStateID != viewModelCreate.FederalStateId)
                {
                    viewModelCreate.UniversityId = null;
                }
            }
            // filters universities by federal state
            if (viewModelCreate.FederalStateId.HasValue)
            {
                viewModelCreate.Universities = new List<SelectListItem>();
                foreach (var item in universities)
                {
                    if (item.FederalStateID == viewModelCreate.FederalStateId)
                    {
                        viewModelCreate.Universities.Add(
                            new SelectListItem { Value = item.UniversityID.ToString(), Text = item.UniversityName });
                    }
                }
            }
            return View(viewModelCreate);
        }

        /// <summary>
        /// Creates new DtQuestion Object with the Properties from the View and Saves it to the Database
        /// </summary>
        /// <param name="viewModelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewEntry(ViewModelCreate viewModelCreate)
        {
            //viewModelSearch = GetViewModelSearch(viewModelSearch).Result;

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

                _context.Add(question);

                _context.SaveChanges();

                //return RedirectToAction("Done");
                return RedirectToAction("ViewQuestion", new { id = question.QuestionID });
            }

            // if not all entries are correct you are redirected
            return View("CreateQuestion", viewModelCreate);
        }
        #endregion

        #region GetViewModels
        private async Task<ViewModelQuestion> GetViewModelQuestions(ViewModelQuestion viewModelQuestion, bool hasBeenLoaded)
        {
            List<DtQuestion> questions = null;

            if (hasBeenLoaded)
            {
                questions = await _context.Questions.ToListAsync();
            }
            else
            {
                questions = await _context.Questions
                .Include(q => q.FieldOfStudies)
                .Include(q => q.Subject)
                .Include(q => q.University)
                .ThenInclude(u => u.FederalState)
                .Include(u => u.University.Country)
                .Include(q => q.Answers)
                .ToListAsync();
            }

            viewModelQuestion.Questions = questions;

            //foreach (var item in questions)
            //{
            //    viewModelQuestion.QuestionsSelectList.Add(new SelectListItem()
            //    {
            //        Value = item.QuestionID.ToString(),
            //        Text = item.Content
            //    });
            //}

            return viewModelQuestion;
        }

        private async Task<ViewModelSearch> GetViewModelSearch(ViewModelSearch viewModelSearch)
        {
            var universities = await _context.Universities.OrderBy(u => u.UniversityName).ToListAsync();
            var federalStates = await _context.FederalStates.OrderBy(f => f.FederalStateName).ToListAsync();
            var subjects = await _context.Subjects.OrderBy(s => s.SubjectName).ToListAsync();
            var fieldsOfStudies = await _context.FieldsOfStudies.OrderBy(f => f.FieldOfStudiesName).ToListAsync();

            //var viewModelSearch = new ViewModelSearch();
            //viewModelSearch.Universities = universities;

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

        private async Task<ViewModelCreate> GetViewModelCreate(ViewModelCreate viewModelCreate)
        {
            var universities = await _context.Universities.ToListAsync();
            var federalStates = await _context.FederalStates.ToListAsync();
            var subjects = await _context.Subjects.ToListAsync();
            var fieldsOfStudies = await _context.FieldsOfStudies.ToListAsync();

            //var viewModelSearch = new ViewModelSearch();
            //viewModelCreate.Universities = universities;

            foreach (var item in universities)
            {
                viewModelCreate.Universities.Add(new SelectListItem()
                {
                    Value = item.UniversityID.ToString(),
                    Text = item.UniversityName
                });
            }
            foreach (var item in federalStates)
            {
                viewModelCreate.FederalStates.Add(new SelectListItem()
                {
                    Value = item.FederalStateID.ToString(),
                    Text = item.FederalStateName
                });
            }
            foreach (var item in subjects)
            {
                viewModelCreate.Subjects.Add(new SelectListItem()
                {
                    Value = item.SubjectID.ToString(),
                    Text = item.SubjectName
                });
            }
            foreach (var item in fieldsOfStudies)
            {
                viewModelCreate.FieldsOfStudies.Add(new SelectListItem()
                {
                    Value = item.FieldOfStudiesID.ToString(),
                    Text = item.FieldOfStudiesName
                });
            }
            return viewModelCreate;
        }
        #endregion

        #region Temporary
        public IActionResult Done(ViewModelSearch viewModelSearch)
        {
            var result = viewModelSearch;
            return View(result);
        }
        #endregion

        #region Error
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region new Field Of Studies, Subject, University

        public IActionResult CreateNewFieldOfStudies(ViewModelCreate viewModelCreate)
        {
            ViewModelCreateFieldOfStudies vmFieldOfStudies = new ViewModelCreateFieldOfStudies();

            return View("CreateFieldOfStudies", vmFieldOfStudies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewFieldOfStudies(ViewModelCreateFieldOfStudies vmFieldOfStudies)
        {
            if (ModelState.IsValid)
            {
                DtFieldOfStudies fieldOfStudies = new DtFieldOfStudies();
                fieldOfStudies.FieldOfStudiesName = vmFieldOfStudies.FieldOfStudiesName;

                _context.Add(fieldOfStudies);
                await _context.SaveChangesAsync();

                ViewModelCreate viewModelCreate = new ViewModelCreate();
                viewModelCreate = GetViewModelCreate(viewModelCreate).Result;

                return View("CreateQuestion", viewModelCreate);
            }

            return View("CreateFieldOfStudies", vmFieldOfStudies);
        }

        public IActionResult CreateNewSubject(ViewModelCreate viewModelCreate)
        {
            ViewModelCreateSubject vmSubject = new ViewModelCreateSubject();

            return View("CreateSubject", vmSubject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewSubject(ViewModelCreateSubject vmSubject)
        {
            if (ModelState.IsValid)
            {
                DtSubject subject = new DtSubject();
                subject.SubjectName = vmSubject.SubjectName;

                _context.Add(subject);
                await _context.SaveChangesAsync();

                ViewModelCreate viewModelCreate = new ViewModelCreate();
                viewModelCreate = GetViewModelCreate(viewModelCreate).Result;

                return View("CreateQuestion", viewModelCreate);
            }

            return View("CreateSubject", vmSubject);
        }

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

            return View("CreateUniversity", vmUniversity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewUniversity(ViewModelCreateUniversity vmUniversity)
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
                viewModelCreate = GetViewModelCreate(viewModelCreate).Result;

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

            return View("CreateUniversity", vmUniversity);
        }

        public IActionResult CancelNewField()
        {
            ViewModelCreate viewModelCreate = new ViewModelCreate();
            viewModelCreate = GetViewModelCreate(viewModelCreate).Result;

            return View("CreateQuestion", viewModelCreate);
        }

        #endregion
    }
}
