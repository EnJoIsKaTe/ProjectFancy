using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XamPass.Models.DataBaseModels;

namespace XamPass.Controllers
{
    //[RequireHttps]
    [Authorize]
    public class DtUniversitiesController : Controller
    {
        private readonly DataContext _context;

        public DtUniversitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: DtUniversities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Universities.ToListAsync());
        }

        // GET: DtUniversities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dtUniversity = await _context.Universities
                .SingleOrDefaultAsync(m => m.UniversityID == id);
            if (dtUniversity == null)
            {
                return NotFound();
            }

            return View(dtUniversity);
        }

        // GET: DtUniversities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DtUniversities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniversityID,UniversityName")] DtUniversity dtUniversity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dtUniversity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dtUniversity);
        }

        // GET: DtUniversities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dtUniversity = await _context.Universities.SingleOrDefaultAsync(m => m.UniversityID == id);
            if (dtUniversity == null)
            {
                return NotFound();
            }
            return View(dtUniversity);
        }

        // POST: DtUniversities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UniversityID,UniversityName")] DtUniversity dtUniversity)
        {
            if (id != dtUniversity.UniversityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dtUniversity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DtUniversityExists(dtUniversity.UniversityID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dtUniversity);
        }

        // GET: DtUniversities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dtUniversity = await _context.Universities
                .SingleOrDefaultAsync(m => m.UniversityID == id);
            if (dtUniversity == null)
            {
                return NotFound();
            }

            return View(dtUniversity);
        }

        // POST: DtUniversities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var dtUniversity = await _context.Universities.SingleOrDefaultAsync(m => m.UniversityID == id);
            _context.Universities.Remove(dtUniversity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DtUniversityExists(long id)
        {
            return _context.Universities.Any(e => e.UniversityID == id);
        }
    }
}
