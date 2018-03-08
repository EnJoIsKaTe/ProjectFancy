using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace XamPass.Models.DataBaseModels
{
    /// <summary>
    /// Class that handles the connection to the entity framework database
    /// </summary>
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        #region DbSets

        // Every Table is represented by a DbSet
        public DbSet<DtQuestion> Questions { get; set; }
        public DbSet<DtFederalState> FederalStates { get; set; }
        public DbSet<DtCountry> Countries { get; set; }
        public DbSet<DtUniversity> Universities { get; set; }
        public DbSet<DtFieldOfStudies> FieldsOfStudies { get; set; }
        public DbSet<DtSubject> Subjects { get; set; }
        public DbSet<DtAnswer> Answers { get; set; }

        #endregion

        /// <summary>
        /// Every Table is named according to the schema
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DtQuestion>().ToTable("dt_question");
            modelBuilder.Entity<DtFederalState>().ToTable("dt_federal_state");
            modelBuilder.Entity<DtCountry>().ToTable("dt_Country");
            modelBuilder.Entity<DtUniversity>().ToTable("dt_University");
            modelBuilder.Entity<DtFieldOfStudies>().ToTable("dt_field_of_studies");
            modelBuilder.Entity<DtSubject>().ToTable("dt_subject");
            modelBuilder.Entity<DtAnswer>().ToTable("dt_answer");
        }
    }
}
