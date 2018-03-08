using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Represents a DataSet for an Answer to a question
    /// </summary>
    public class DtAnswer
    {
        #region Properties

        /// <summary>
        /// Identifier of the Dataset
        /// </summary>
        [Key]
        public int AnswerID { get; set; }

        /// <summary>
        /// DateTime when the Answer has been submitted
        /// </summary>
        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// Content of the Answer
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Upvotes to the Answer
        /// </summary>
        public int UpVotes { get; set; }
        
        #endregion
    }
}
