using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Class that represents a field of studies dataset
    /// </summary>
    public class DtFieldOfStudies
    {
        #region Properties

        /// <summary>
        /// Identifier of the dataset
        /// </summary>
        [Key]
        public int FieldOfStudiesID { get; set; }

        /// <summary>
        /// Name of the field of studies dataset
        /// </summary>
        public string FieldOfStudiesName { get; set; }

        /// <summary>
        /// Type of the field of studies
        /// </summary>
        public FieldOfStudiesType Type { get; set; }
        
        #endregion
    }
}
