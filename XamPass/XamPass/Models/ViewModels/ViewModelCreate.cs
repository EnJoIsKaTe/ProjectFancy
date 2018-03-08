using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Models.ViewModels
{
    /// <summary>
    /// ViewModel that stores the data to create a new question
    /// </summary>
    public class ViewModelCreate : DropDownViewModel      
    { 
        [Required(ErrorMessage = "PleaseSelectUniversity")]
        public override int? UniversityId { get; set; }
        [Required(ErrorMessage = "PleaseSelectFederalState")]
        public override int? FederalStateId { get; set; }
        [Required(ErrorMessage = "PleaseSelectSubject")]
        public override int? SubjectId { get; set; }
        [Required(ErrorMessage = "PleaseSelectFieldOfStudies")]
        public override int? FieldOfStudiesId { get; set; }

        // Properties, that have to be filled from the UI
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "PleaseEnterQuestion")]
        public string QuestionContent { get; set; }
        [Required(ErrorMessage = "PleaseEnterTitle")]
        public string QuestionTitle { get; set; }
        [DataType(DataType.MultilineText)]
        public string AnswerContent { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ViewModelCreate()
        {

        }       
    }
}
