using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Klasse die einen Datensatz für eine Prüfungsfrage beschreibt
    /// </summary>
    public class DtQuestion
    {
        #region Properties

        /// <summary>
        /// Id des Datensatzes
        /// </summary>
        [Key]
        public int QuestionID { get; set; }

        /// <summary>
        /// Universität an der die Frage gestellt wurde
        /// </summary>
        public int UniversityID { get; set; }
        public DtUniversity University { get; set; }

        /// <summary>
        /// Studiengang in dem die Frage gestellt wurde
        /// </summary>
        public int FieldOfStudiesID { get; set; }
        public DtFieldOfStudies FieldOfStudies { get; set; }

        /// <summary>
        /// Fach in dem die Frage gestellt wurde
        /// </summary>
        public int SubjectID { get; set; }
        public DtSubject Subject { get; set; }

        /// <summary>
        /// Liste von Antworten, die auf die Frage gegeben wurden
        /// </summary>
        //public int AnswerID { get; set; }
        public List<DtAnswer> Answers { get; set; }

        /// <summary>
        /// Zeitpunkt der Erstellung des Datensatzes
        /// </summary>
        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// Titel / Betreff, der Frage
        /// </summary>
        [StringLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Inhaltstext der Frage
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Anzahl der positiven Bewertungen der Frage
        /// </summary>
        public int UpVotes { get; set; }        

        #endregion
    }
}
