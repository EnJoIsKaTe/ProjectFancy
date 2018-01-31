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
    public class DtCountry
    {
        #region Properties

        /// <summary>
        /// Identifier des Datensatzes
        /// </summary>
        [Key]
        public long CountryID { get; set; }

        /// <summary>
        /// Name des Staates
        /// </summary>
        [StringLength(50)]
        public string CountryName { get; set; }

        #endregion         
    }
}
