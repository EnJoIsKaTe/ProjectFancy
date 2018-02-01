using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace XamPass.Models.DataBaseModels
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        #region DbSets

        // Jede DatenbankTabelle wird durch ein DbSet repräsentiert
        public DbSet<DtQuestion> Questions { get; set; }
        public DbSet<DtFederalState> FederalStates { get; set; }
        public DbSet<DtCountry> Countries { get; set; }
        public DbSet<DtUniversity> Universities { get; set; }
        public DbSet<DtFieldOfStudies> FieldsOfStidies { get; set; }
        public DbSet<DtSubject> Subjects { get; set; }
        public DbSet<DtAnswer> Answers { get; set; }

        #endregion

        /// <summary>
        /// Jede Datenbanktabelle wird nach dem Namensschema benannt
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
