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
    /// ViewModel that provides the data to search for Questions
    /// </summary>
    public class ViewModelSearch : DropDownViewModel
    {
        // show questions
        public int? QuestionId { get; set; }
        public List<DtQuestion> Questions { get; set; }
        public bool SearchExecuted { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ViewModelSearch()
        {
            Questions = new List<DtQuestion>();
        }
    }
}
