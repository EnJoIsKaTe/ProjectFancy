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
    public class ViewModelCreate : DropDownViewModel
    {
        [Required(ErrorMessage = "Bitte geben Sie eine Universität an")]
        public override int? UniversityId { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie ein Bundesland an")]
        public override int? FederalStateId { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie ein Fach an")]
        public override int? SubjectId { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie einen Studiengang an")]
        public override int? FieldOfStudiesId { get; set; }

        // Properties, die aus der Oberfläche zu befüllen sind
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Bitte geben Sie eine Frage ein")]
        public string QuestionContent { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie einen Titel ein")]
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
