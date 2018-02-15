using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Models
{
    /// <summary>
    /// ViewModel that provides the data to search for Questions
    /// </summary>
    public class ViewModelSearch
    {
        public int UniversityId { get; set; }
        public int FederalStateId { get; set; }
        public int SubjectId { get; set; }
        public int FieldOfStudiesId { get; set; }

        // Properties, die aus der Oberfläche zu befüllen sind
        [DataType(DataType.MultilineText)]
        public string QuestionContent { get; set; }
        public string QuestionTitle { get; set; }

        public List<DtUniversity> Universities { get; set; }
        //public List<DtUniversity> UniversitiesFiltered { get; set; }
        public List<SelectListItem> UniversitySelectList { get; set; }
        public List<SelectListItem> FederalStates { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<SelectListItem> FieldsOfStudies { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ViewModelSearch()
        {
            Universities = new List<DtUniversity>();
            UniversitySelectList = new List<SelectListItem>();
            FederalStates = new List<SelectListItem>();
            Subjects = new List<SelectListItem>();
            FieldsOfStudies = new List<SelectListItem>();
        }
    }
}
