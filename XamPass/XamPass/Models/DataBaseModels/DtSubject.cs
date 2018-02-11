using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Klasse die einen Datensatz für ein Fach beschreibt
    /// </summary>
    public class DtSubject
    {
        #region Prperties

        /// <summary>
        /// Identifier des Datensatzes
        /// </summary>
        [Key]
        public int SubjectID { get; set; }

        /// <summary>
        /// Name des Faches
        /// </summary>
        [StringLength(100)]
        public string SubjectName { get; set; }

        #endregion
    }
}
