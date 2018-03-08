using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.ViewModels
{
    /// <summary>
    /// ViewModel to create a new subject
    /// </summary>
    public class ViewModelCreateSubject
    {
        /// <summary>
        /// Name of the Subject
        /// </summary>
        [StringLength(100)]
        [MinLength(5, ErrorMessage = "MinLength")]
        [Required(ErrorMessage = "PleaseEnterSubject")]
        public string SubjectName { get; set; }
    }
}
