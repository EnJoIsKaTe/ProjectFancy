using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Class that represents a federal state dataset
    /// </summary>
    public class DtFederalState
    {
        #region Properties

        /// <summary>
        /// Identifier of the dataset
        /// </summary>
        [Key]
        public int FederalStateID { get; set; }

        /// <summary>
        /// Name of the federal state
        /// </summary>
        public string FederalStateName { get; set; }

        #endregion
    }
}
