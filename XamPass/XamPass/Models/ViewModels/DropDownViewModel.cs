using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Models.ViewModels
{
    /// <summary>
    /// ViewModel - Class that holds the properties for the Index View, to search for questions in the database and the View to create new Questions
    /// </summary>
    public class DropDownViewModel
    {
        public virtual int? UniversityId { get; set; }
        public virtual int? FederalStateId { get; set; }
        public virtual int? SubjectId { get; set; }
        public virtual int? FieldOfStudiesId { get; set; }

        public List<SelectListItem> Universities { get; set; }
        public List<SelectListItem> FederalStates { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<SelectListItem> FieldsOfStudies { get; set; }

        /// <summary>
        /// Standard Constructor
        /// </summary>
        public DropDownViewModel()
        {
            Universities = new List<SelectListItem>();
            FederalStates = new List<SelectListItem>();
            Subjects = new List<SelectListItem>();
            FieldsOfStudies = new List<SelectListItem>();
        }

        /// <summary>
        /// Fills all the Dropdown lists with Data from the database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public void FillAllDropdowns(DataContext context, ILogger logger)
        {
            List<DtFieldOfStudies> fieldsOfStudies = new List<DtFieldOfStudies>();
            List<DtSubject> subjects = new List<DtSubject>();
            List<DtFederalState> federalStates = new List<DtFederalState>();
            List<DtUniversity> universities = new List<DtUniversity>();

            try
            {
                // get entries from db            
                fieldsOfStudies = context.FieldsOfStudies.OrderBy(f => f.FieldOfStudiesName).ToList();
                subjects = context.Subjects.OrderBy(s => s.SubjectName).ToList();
                federalStates = context.FederalStates.OrderBy(f => f.FederalStateName).ToList();
                universities = context.Universities.OrderBy(u => u.UniversityName).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while loading Dropdown entries from the Database");
            }

            // fill SelectLists
            foreach (var item in fieldsOfStudies)
            {
                FieldsOfStudies.Add(new SelectListItem
                {
                    Value = item.FieldOfStudiesID.ToString(),
                    Text = item.FieldOfStudiesName
                });
            }
            foreach (var item in subjects)
            {
                Subjects.Add(new SelectListItem
                {
                    Value = item.SubjectID.ToString(),
                    Text = item.SubjectName
                });
            }
            foreach (var item in federalStates)
            {
                FederalStates.Add(new SelectListItem
                {
                    Value = item.FederalStateID.ToString(),
                    Text = item.FederalStateName
                });
            }
            foreach (var item in universities)
            {
                Universities.Add(new SelectListItem
                {
                    Value = item.UniversityID.ToString(),
                    Text = item.UniversityName
                });
            }
        }

        /// <summary>
        /// Filters the Universities by the selected federal state
        /// </summary>
        /// <param name="context">database connection</param>
        /// <param name="logger">Logger interface</param>
        public void FilterUniversitiesByFederalState(DataContext context, ILogger logger)
        {
            List<DtUniversity> universities = new List<DtUniversity>();
            try
            {
                universities = context.Universities.OrderBy(u => u.UniversityName).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while loading Dropdown entries from the Database");
            }

            // sets selected University to null if the seleected FederalState does not fit
            if (UniversityId.HasValue)
            {
                var university = universities.FirstOrDefault(u => u.UniversityID == UniversityId);
                if (university.FederalStateID != FederalStateId)
                {
                    UniversityId = null;
                }
            }
            // filter Universities by selected FederalState
            if (FederalStateId.HasValue)
            {
                Universities = new List<SelectListItem>();
                foreach (var item in universities)
                {
                    if (item.FederalStateID == FederalStateId)
                    {
                        Universities.Add(
                            new SelectListItem { Value = item.UniversityID.ToString(), Text = item.UniversityName });
                    }
                }
            }
        }
    }
}
