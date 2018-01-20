using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XamPass.Models;

namespace XamPass.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FirstTest()
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Institutions = new List<Institution>()
            {
                new Institution(){Id=0, Name="BA Leipzig"},
                new Institution(){Id=1, Name="HTWK Leipzig"},
                new Institution(){Id=2, Name="Universität Leipzig"},
                new Institution(){Id=3, Name="BA Glauchau"},
                new Institution(){Id=4, Name="BA Dresden"},
                new Institution(){Id=5, Name="HfM Weimar"},
                new Institution(){Id=6, Name="Universität Jena"},
                new Institution(){Id=7, Name="Fernuniversität Hagen"}
            };
            //List<string> ObjList = new List<string>()
            //{
            //    "Latur",
            //    "Mumbai",
            //    "Pune",
            //    "Delhi",
            //    "Dehradun",
            //    "Noida",
            //    "New Delhi"
            //};
            return View(viewModel);
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
