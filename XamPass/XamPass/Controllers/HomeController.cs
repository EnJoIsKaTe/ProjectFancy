﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public HomeController (DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateDB()
        {

            // call method
            DBInitialize.DatabaseTest(_context);
            return RedirectToAction("Done");
        }

        [HttpGet]
        public async Task<IActionResult> FirstTest()
        {
            var universities = await _context.Universities.ToListAsync();
            var model = new FirstModel();
            model.Institutions = new List<SelectListItem>();
            foreach(var item in universities)
            {
                model.Institutions.Add(new SelectListItem() { Value = item.UniversityID.ToString(), Text = item.UniversityName });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FirstTest(FirstModel model)
        {
            var universities = await _context.Universities.ToListAsync();
            if (ModelState.IsValid)
            {
                var university = universities.First(u => u.UniversityID == model.Institution.Id);
                var result = new Institution() { Id = (int)university.UniversityID, Name = university.UniversityName };
                return RedirectToAction("Done", result);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SecondTest()
        {
            var institutions = GetAllInstitutions();
            var model = new SecondModel();
            model.InstitutionList = (List<Institution>)institutions;
            return View(model);
        }

        [HttpPost]
        public IActionResult SecondTest(string institution)
        {
            var institutions = GetAllInstitutions();
            SecondModel model = new SecondModel();
            model.InstitutionList = (List<Institution>)institutions;
            if (ModelState.IsValid)
            {
                var result = institutions.First(i => i.Name == institution);
                return RedirectToAction("Done", result);
            }
            return View(model);
        }

        public IActionResult Done(Institution institution)
        {
            return View(institution);
        }

        private IEnumerable<Institution> GetAllInstitutions()
        {
            List<Institution> institutions = new List<Institution>();
            institutions.Add(new Institution() { Id = 0, Name = "BA Leipzig" });
            institutions.Add(new Institution() { Id = 1, Name = "HTWK Leipzig" });
            institutions.Add(new Institution() { Id = 2, Name = "Universität Leipzig" });
            institutions.Add(new Institution() { Id = 3, Name = "BA Glauchau" });
            institutions.Add(new Institution() { Id = 4, Name = "BA Dresden" });
            institutions.Add(new Institution() { Id = 5, Name = "HfM Weimar" });
            institutions.Add(new Institution() { Id = 6, Name = "Universität Jena" });
            institutions.Add(new Institution() { Id = 7, Name = "Fernuniversität Hagen" });
            return institutions;
        }

        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<Institution> elements)
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem { Value = element.Id.ToString(), Text = element.Name });
            }
            return selectList;
        }









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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
