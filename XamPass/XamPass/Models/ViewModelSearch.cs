using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models
{
    public class ViewModelSearch
    {
        public int UniversityId { get; set; }
        public int FederalStateId { get; set; }
        public int SubjectId { get; set; }
        public int FieldOfStudiesId { get; set; }

        public List<SelectListItem> Universities { get; set; }
        public List<SelectListItem> FederalStates { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<SelectListItem> FieldsOfStudies { get; set; }

        public ViewModelSearch()
        {
            Universities = new List<SelectListItem>();
            FederalStates = new List<SelectListItem>();
            Subjects = new List<SelectListItem>();
            FieldsOfStudies = new List<SelectListItem>();
        }
    }
}
