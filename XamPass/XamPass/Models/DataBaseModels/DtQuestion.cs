using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Class that represents a dataset for a Exam-Question
    /// </summary>
    public class DtQuestion
    {
        #region Properties

        /// <summary>
        /// Identifier of the question
        /// </summary>
        [Key]
        public int QuestionID { get; set; }

        /// <summary>
        /// Foreign Key of the University where the Question was asked
        /// </summary>
        public int? UniversityID { get; set; }
        public DtUniversity University { get; set; }

        /// <summary>
        /// Foreign Key of Field of Studies where the Question has been asked
        /// </summary>
        public int FieldOfStudiesID { get; set; }
        public DtFieldOfStudies FieldOfStudies { get; set; }

        /// <summary>
        /// Foreign Key of Subject where the Question has been asked
        /// </summary>
        public int SubjectID { get; set; }
        public DtSubject Subject { get; set; }

        /// <summary>
        /// Foreign Key List of answers that have been given to this question
        /// </summary>
        //public int AnswerID { get; set; }       
        public List<DtAnswer> Answers { get; set; }

        /// <summary>
        /// Date when the question has been submitted
        /// </summary>
        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// Title of the question
        /// </summary>        
        [StringLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Content of the question
        /// </summary>        
        public string Content { get; set; }

        /// <summary>
        /// positive votes for the quality of the question
        /// </summary>
        public int UpVotes { get; set; }        

        #endregion

        /// <summary>
        /// Standard Construcor
        /// </summary>
        public DtQuestion()
        {
            Answers = new List<DtAnswer>();
        }
    }
}
