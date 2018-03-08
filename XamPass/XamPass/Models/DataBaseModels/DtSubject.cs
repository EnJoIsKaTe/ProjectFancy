using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Class that represents a subject dataset
    /// </summary>
    public class DtSubject
    {
        #region Prperties

        /// <summary>
        /// Identifier of the subject dataset
        /// </summary>
        [Key]
        public int SubjectID { get; set; }

        /// <summary>
        /// Name of the Subject
        /// </summary>
        public string SubjectName { get; set; }

        #endregion
    }
}
