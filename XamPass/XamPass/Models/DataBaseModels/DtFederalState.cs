using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Klasse die einen Datensatz für ein Bundesland (Bundesstaat) beschreibt
    /// </summary>
    public class DtFederalState
    {
        #region Properties

        /// <summary>
        /// Identifier des Datensatzes
        /// </summary>
        [Key]
        public long FederalStateID { get; set; }

        /// <summary>
        /// Name des Bundesstaates
        /// </summary>
        [StringLength(100)]
        public string FederalStateName { get; set; }

        #endregion
    }
}
