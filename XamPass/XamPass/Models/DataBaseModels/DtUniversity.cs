using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Klasse die einen Datensatz für einen Staat beschreibt
    /// </summary>
    public class DtUniversity
    {
        #region Properties

        /// <summary>
        /// Identifier des Datensatzes
        /// </summary>
        [Key]
        public long UniversityID { get; set; }

        /// <summary>
        /// FK für den Staat der Universität
        /// </summary>
        public DtCountry Country { get; set; }

        /// <summary>
        /// FK für den Bundesstaat der Universität
        /// </summary>
        public DtFederalState FederalState { get; set; }

        /// <summary>
        /// Name der Hochschule / Universität
        /// </summary>
        [StringLength(100)]
        public string UniversityName { get; set; }

        #endregion
    }
}
