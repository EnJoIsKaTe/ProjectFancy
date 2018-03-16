using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Represents a dataset to a country
    /// </summary>
    public class DtCountry
    {
        #region Properties

        /// <summary>
        /// Identifier of the dataset
        /// </summary>
        [Key]        
        public int CountryID { get; set; }

        /// <summary>
        /// Name of the country
        /// </summary>
        [StringLength(100)]
        public string CountryName { get; set; }

        #endregion         
    }
}
