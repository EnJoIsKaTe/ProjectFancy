using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.ViewModels
{
    public class ViewModelCreateUniversity
    {
        /// <summary>
        /// Name of the University
        /// </summary>
        [StringLength(100)]
        [MinLength(5, ErrorMessage = "Der Name der Universität muss mindestens 5 Zeichen haben")]
        [Required(ErrorMessage = "Bitte geben Sie eine Universität an")]
        public string UniversityName { get; set; }

        /// <summary>
        /// Identifier of the Federal State
        /// </summary>
        [Required(ErrorMessage = "Bitte geben Sie ein Bundesland an")]
        public int? FederalStateId { get; set; }
        
        /// <summary>
        /// List of Federal States from the Database
        /// </summary>
        public List<SelectListItem> FederalStates { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ViewModelCreateUniversity()
        {
            FederalStates = new List<SelectListItem>();
        }

    }
}
