using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using XamPass.Models;
using XamPass.Models.DataBaseModels;

namespace XamPass.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _Context;

        public HomeController (DataContext context)
        {
            _Context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateDB()
        {
            
            // call method
            return RedirectToAction("Done");
        }

        [HttpGet]
        public IActionResult FirstTest()
        {
            var institutions = GetAllInstitutions();
            var model = new FirstModel();
            model.Institutions = GetSelectListItems(institutions);

            return View(model);
        }

        [HttpPost]
        public IActionResult FirstTest(FirstModel model)
        {
            //var result = model;

            var institutions = GetAllInstitutions();
            model.Institutions = GetSelectListItems(institutions);
            if(ModelState.IsValid)
            {
                var result = institutions.First(i => i.Id == model.Institution.Id);
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

            //List<string> result = new List<string>();
            //foreach(var item in institutions)
            //{
            //    result.Add(item.Name);
            //}
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
