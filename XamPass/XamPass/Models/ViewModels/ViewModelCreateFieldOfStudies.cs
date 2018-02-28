using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.ViewModels
{
    public class ViewModelCreateFieldOfStudies
    {
        /// <summary>
        /// Name of the FieldOfStudies
        /// </summary>
        [StringLength(100)]
        [MinLength(5, ErrorMessage = "Der Name der Fachrichtung muss mindestens 5 Zeichen haben")]
        [Required(ErrorMessage = "Bitte geben Sie eine Fachrichtung an")]
        public string FieldOfStudiesName { get; set; }
    }
}
