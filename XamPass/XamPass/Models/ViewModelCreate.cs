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

        // Properties, die aus der Oberfläche zu befüllen sind
        public string QuestionContent { get; set; }
        public string QuestionTitle { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ViewModelCreate(ViewModelSearch viewModelSearch)
        {
            UniversityId = viewModelSearch.UniversityId;
            FederalStateId = viewModelSearch.FederalStateId;
            CountryId = 0;
            SubjectId = viewModelSearch.SubjectId;
            FieldOfStudiesId = viewModelSearch.FieldOfStudiesId;
        }       
    }
}
