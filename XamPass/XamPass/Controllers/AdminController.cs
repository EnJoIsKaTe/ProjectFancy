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

namespace XamPass.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        private readonly IStringLocalizer<AdminController> _stringLocalizer;

        public AdminController(DataContext context, IStringLocalizer<AdminController> stringLocalizer)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

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
        
        public async Task<IActionResult> IndexQuestions()
        {
            var dataContext = _context.Questions.Include(d => d.FieldOfStudies).Include(d => d.Subject).Include(d => d.University);
            return View(await dataContext.ToListAsync());
        }
        
        public async Task<IActionResult> DetailsQuestions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        
        public IActionResult CreateQuestions()
        {
            ViewData["FieldOfStudiesID"] = new SelectList(_context.FieldsOfStudies, "FieldOfStudiesID", "FieldOfStudiesID");
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectID");
            ViewData["UniversityID"] = new SelectList(_context.Universities, "UniversityID", "UniversityID");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestions([Bind("QuestionID,UniversityID,FieldOfStudiesID,SubjectID,SubmissionDate,Title,Content,UpVotes")] DtQuestion dtQuestion)
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
            return View(dtQuestion);
        }
        
        public async Task<IActionResult> EditQuestions(int? id)
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestions(int id, [Bind("QuestionID,UniversityID,FieldOfStudiesID,SubjectID,SubmissionDate,Title,Content,UpVotes")] DtQuestion dtQuestion)
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
            return View(dtQuestion);
        }
        
        public async Task<IActionResult> DeleteQuestions(int? id)
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
        
        [HttpPost, ActionName("DeleteQuestions")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestionsConfirmed(int id)
        {
            var dtQuestion = await _context.Questions
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(m => m.QuestionID == id);               

            List<DtAnswer> answers = dtQuestion.Answers;
            _context.Answers.RemoveRange(answers);

            _context.Questions.Remove(dtQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexQuestions));
        }

        private bool DtQuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionID == id);
        }

        #endregion
    }
}