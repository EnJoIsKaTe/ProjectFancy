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
using XamPass.Models.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace XamPass.Controllers
{
    /// <summary>
    /// Controller for a logged in Admin to manage the application and Database
    /// </summary>
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DataContext _context;
        private readonly IStringLocalizer<AdminController> _stringLocalizer;
        private ILogger _logger;

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="context">Database Connection inserted via Dependency injection</param>
        /// <param name="stringLocalizer">Localization interface, inserted via Dependendy injection</param>
        public AdminController(DataContext context, IStringLocalizer<AdminController> stringLocalizer, ILogger<AdminController> logger)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
            _logger = logger;
        }

        /// <summary>
        /// Index Page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method that Creates the Database and seeds it with data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CreateDB()
        {
            var result = DBInitialize.SeedDatabase(_context);
            CreateDBViewModel createDBViewModel = new CreateDBViewModel();
            if (result)
            {
                createDBViewModel.Finished = _stringLocalizer["DataEntered"];
            }
            else
            {
                createDBViewModel.Finished = _stringLocalizer["NoDataEntered"];
            }
            return View(createDBViewModel);
        }

        #region Review Questions

        /// <summary>
        /// Loads the questions from the Database
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> IndexQuestions()
        {
            try
            {
                var dataContext = _context.Questions.Include(d => d.FieldOfStudies).Include(d => d.Subject).Include(d => d.University);
                return View(await dataContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading questions from the database");
                return View(null);
            }
        }

        /// <summary>
        /// Show the Questions with details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DetailsQuestions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dtQuestion = await _context.Questions
                    .Include(d => d.FieldOfStudies)
                    .Include(d => d.Subject)
                    .Include(d => d.University)
                    .SingleOrDefaultAsync(m => m.QuestionID == id);
                if (dtQuestion == null)
                {
                    return NotFound();
                }
                return View(dtQuestion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading questions from the Database");
                return View(null);
            }
        }

        /// <summary>
        /// Creates all questions by loading the from the Database
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateQuestions()
        {
            try
            {
                ViewData["FieldOfStudiesID"] = new SelectList(_context.FieldsOfStudies, "FieldOfStudiesID", "FieldOfStudiesID");
                ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectID");
                ViewData["UniversityID"] = new SelectList(_context.Universities, "UniversityID", "UniversityID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading data from the database");
            }

            return View();
        }

        /// <summary>
        /// Creates Questions from the Database
        /// </summary>
        /// <param name="dtQuestion"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestions([Bind("QuestionID,UniversityID,FieldOfStudiesID,SubjectID,SubmissionDate,Title,Content,UpVotes")] DtQuestion dtQuestion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(dtQuestion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(IndexQuestions));
                }
                ViewData["FieldOfStudiesID"] = new SelectList(_context.FieldsOfStudies, "FieldOfStudiesID", "FieldOfStudiesID", dtQuestion.FieldOfStudiesID);
                ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectID", dtQuestion.SubjectID);
                ViewData["UniversityID"] = new SelectList(_context.Universities, "UniversityID", "UniversityID", dtQuestion.UniversityID);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while loading data from the database");
            }
            return View(dtQuestion);
        }

        /// <summary>
        /// Edit the loaded Questions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditQuestions(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var dtQuestion = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionID == id);
                if (dtQuestion == null)
                {
                    return NotFound();
                }
                ViewData["FieldOfStudiesID"] = new SelectList(_context.FieldsOfStudies, "FieldOfStudiesID", "FieldOfStudiesID", dtQuestion.FieldOfStudiesID);
                ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectID", dtQuestion.SubjectID);
                ViewData["UniversityID"] = new SelectList(_context.Universities, "UniversityID", "UniversityID", dtQuestion.UniversityID);
                return View(dtQuestion);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while loading data from the database");
                return View(new DtQuestion());
            }
        }

        /// <summary>
        /// Edits the selected Question
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dtQuestion"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestions(int id, [Bind("QuestionID,UniversityID,FieldOfStudiesID,SubjectID,SubmissionDate,Title,Content,UpVotes")] DtQuestion dtQuestion)
        {
            try
            {
                if (id != dtQuestion.QuestionID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(dtQuestion);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DtQuestionExists(dtQuestion.QuestionID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(IndexQuestions));
                }
                ViewData["FieldOfStudiesID"] = new SelectList(_context.FieldsOfStudies, "FieldOfStudiesID", "FieldOfStudiesID", dtQuestion.FieldOfStudiesID);
                ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectID", dtQuestion.SubjectID);
                ViewData["UniversityID"] = new SelectList(_context.Universities, "UniversityID", "UniversityID", dtQuestion.UniversityID);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while loading data from the database");
            }

            return View(dtQuestion);
        }

        /// <summary>
        /// Delete the selected question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteQuestions(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var dtQuestion = await _context.Questions
                    .Include(d => d.FieldOfStudies)
                    .Include(d => d.Subject)
                    .Include(d => d.University)
                    .Include(d => d.Answers)
                    .SingleOrDefaultAsync(m => m.QuestionID == id);
                if (dtQuestion == null)
                {
                    return NotFound();
                }

                return View(dtQuestion);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while loading data from the database");
            }

            return View(new DtQuestion());
        }

        /// <summary>
        /// Deletes the question if it is confirmed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteQuestions")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestionsConfirmed(int id)
        {
            try
            {
                var dtQuestion = await _context.Questions
                    .Include(q => q.Answers)
                    .SingleOrDefaultAsync(m => m.QuestionID == id);

                List<DtAnswer> answers = dtQuestion.Answers;
                _context.Answers.RemoveRange(answers);

                _context.Questions.Remove(dtQuestion);
                await _context.SaveChangesAsync();                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading data from the database");
            }

            return RedirectToAction(nameof(IndexQuestions));
        }

        /// <summary>
        /// Question does not exists returns question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool DtQuestionExists(int id)
        {
            try
            {
                return _context.Questions.Any(e => e.QuestionID == id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while loading data from the database");
            }

            return true;
        }
        
        #endregion
    }
}