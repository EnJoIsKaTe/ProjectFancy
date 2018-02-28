using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Models.ViewModels
{
    public class ViewModelQuestion
    {
        // TODO: nicht genutzte Properties löschen
        public int? QuestionId { get; set; }
        public DtQuestion Question { get; set; }
        public DtAnswer Answer { get; set; }
        public DtFieldOfStudies FieldOfStudies { get; set; }
        public DtSubject Subject { get; set; }
        public DtUniversity University { get; set; }
        public DtCountry Country { get; set; }
        public DtFederalState FederalState { get; set; }

        public List<DtQuestion> Questions { get; set; }
        public List<DtAnswer> Answers { get; set; }

        public ViewModelQuestion()
        {
            Questions = new List<DtQuestion>();
            Answers = new List<DtAnswer>();
        }
    }
}
