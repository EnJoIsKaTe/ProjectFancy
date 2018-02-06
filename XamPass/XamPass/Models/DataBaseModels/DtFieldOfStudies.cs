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
        public long FieldOfStudiesID { get; set; }

        /// <summary>
        /// Name des Studienfachs
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Typ des Studienfachs (ermöglicht eine später Spezifizierung)
        /// </summary>
        [StringLength(50)]
        public string Type { get; set; }
        
        #endregion
    }
}
