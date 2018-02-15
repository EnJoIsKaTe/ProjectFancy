using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Klasse die einen Datensatz für ein Studienfach beschreibt
    /// </summary>
    public class DtFieldOfStudies
    {
        #region Properties

        /// <summary>
        /// Identifier des Datensatzes
        /// </summary>
        [Key]
        public int FieldOfStudiesID { get; set; }

        /// <summary>
        /// Name des Studienfachs
        /// </summary>
        [StringLength(100)]
        public string FieldOfStudiesName { get; set; }

        /// <summary>
        /// Typ des Studienfachs (ermöglicht eine später Spezifizierung)
        /// </summary>
        public FieldOfStudiesType Type { get; set; }
        
        #endregion
    }
}
