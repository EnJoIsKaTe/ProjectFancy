using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Klasse die einen Datensatz für eine Antwort beschreibt
    /// </summary>
    public class DtAnswer
    {
        #region Properties

        /// <summary>
        /// Identifier des Datensatzes
        /// </summary>
        [Key]
        public long AnswerID { get; set; }

        /// <summary>
        /// Zeitpunkt der Erstellung des Datensatzes
        /// </summary>
        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// Inhalt der Antwort
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Positive Bewertungen für die Antwort
        /// </summary>
        public int UpVotes { get; set; }

        #endregion
    }
}
