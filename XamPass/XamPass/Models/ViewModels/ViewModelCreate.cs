using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Models.ViewModels
{
    public class ViewModelCreate
    {
        [Required(ErrorMessage = "Bitte geben Sie eine Universität an")]
        public int? UniversityId { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie ein Bundesland an")]
        public int? FederalStateId { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie ein Fach an")]
        public int? SubjectId { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie einen Studiengang an")]
        public int? FieldOfStudiesId { get; set; }

        // Properties, die aus der Oberfläche zu befüllen sind
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Bitte geben Sie eine Frage ein")]
        public string QuestionContent { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie einen Titel ein")]
        public string QuestionTitle { get; set; }
        [DataType(DataType.MultilineText)]
        public string AnswerContent { get; set; }

        public List<SelectListItem> Universities { get; set; }
        public List<SelectListItem> FederalStates { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<SelectListItem> FieldsOfStudies { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ViewModelCreate()
        {
            Universities = new List<SelectListItem>();
            FederalStates = new List<SelectListItem>();
            Subjects = new List<SelectListItem>();
            FieldsOfStudies = new List<SelectListItem>();
        }       
    }
}
