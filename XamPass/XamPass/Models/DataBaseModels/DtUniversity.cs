using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Class that represents a university dataset
    /// </summary>
    public class DtUniversity
    {
        #region Properties

        /// <summary>
        /// Identifier of the Dataset
        /// </summary>
        [Key]
        public int UniversityID { get; set; }

        /// <summary>
        /// Foreign key for the Country where the University is located
        /// </summary>
        public int CountryID { get; set; }
        public DtCountry Country { get; set; }

        /// <summary>
        /// Foreign key for the federal state where the University is located
        /// </summary>
        public int FederalStateID { get; set; }
        public DtFederalState FederalState { get; set; }

        /// <summary>
        /// Name of the University
        /// </summary>
        [StringLength(100)]
        public string UniversityName { get; set; }

        #endregion
    }
}
