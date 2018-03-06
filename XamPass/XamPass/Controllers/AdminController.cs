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

namespace XamPass.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateDB()
        {
            DBInitialize.SeedDatabase(_context);
            return RedirectToAction("Done");
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Done(ViewModelSearch viewModelSearch)
        {
            var result = viewModelSearch;
            return View(result);
        }

        #region Review Questions

        // GET: DtQuestions
        public async Task<IActionResult> IndexQuestions()
        {
            var dataContext = _context.Questions.Include(d => d.FieldOfStudies).Include(d => d.Subject).Include(d => d.University);
            return View(await dataContext.ToListAsync());
        }

        // GET: DtQuestions/Details/5
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

        // GET: DtQuestions/Create
        public IActionResult CreateQuestions()
        {
            ViewData["FieldOfStudiesID"] = new SelectList(_context.FieldsOfStudies, "FieldOfStudiesID", "FieldOfStudiesID");
            ViewData["SubjectID"] = new SelectList(_context.Subjects, "SubjectID", "SubjectID");
            ViewData["UniversityID"] = new SelectList(_context.Universities, "UniversityID", "UniversityID");
            return View();
        }

        // POST: DtQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: DtQuestions/Edit/5
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

        // POST: DtQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: DtQuestions/Delete/5
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
                .SingleOrDefaultAsync(m => m.QuestionID == id);
            if (dtQuestion == null)
            {
                return NotFound();
            }

            return View(dtQuestion);
        }

        // POST: DtQuestions/Delete/5
        [HttpPost, ActionName("DeleteQuestions")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestionsConfirmed(int id)
        {
            var dtQuestion = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionID == id);
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