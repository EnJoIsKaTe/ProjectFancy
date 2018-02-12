using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Models
{
    public class ViewModelCreate
    {
        public int UniversityId { get; set; }
        public int FederalStateId { get; set; }
        public int CountryId { get; set; }
        public int SubjectId { get; set; }
        public int FieldOfStudiesId { get; set; }

        // Properties die als Vorschläge angezeigt werden können
        public List<SelectListItem> Universities { get; set; }
        public List<SelectListItem> FederalStates { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<SelectListItem> FieldsOfStudies { get; set; }

        // Properties, die aus der Oberfläche zu befüllen sind
        public string QuestionContent { get; set; }
        public string QuestionTitle { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ViewModelCreate()
        {
            Universities = new List<SelectListItem>();
            FederalStates = new List<SelectListItem>();
            Countries = new List<SelectListItem>();
            Subjects = new List<SelectListItem>();
            FieldsOfStudies = new List<SelectListItem>();
        }       
    }
}
