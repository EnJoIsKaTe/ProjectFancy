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
        public int FederalStateID { get; set; }

        /// <summary>
        /// Name des Bundesstaates
        /// </summary>
        [StringLength(100)]
        [MinLength(5,ErrorMessage = "Der Name des Bundesland muss mindestens 5 Zeichen lang sein")]
        [Required(ErrorMessage = "Bitte geben Sie ein Bundesland an")]
        public string FederalStateName { get; set; }

        #endregion
    }
}
