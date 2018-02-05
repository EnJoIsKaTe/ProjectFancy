using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Models
{
    public class DBInitialize
    {
        /// <summary>
        /// Testet die Datenbankverbindung
        /// </summary>
        public static void DatabaseTest(DataContext context)
        {
            // Löscht die Datenbank falls vorhanden
            context.Database.EnsureDeleted();

            // Erstellt die Datenbank neu auf Grundlage der Model-Klassen
            context.Database.EnsureCreated();

            // Testdaten in die Tabelle dt_federal_state einfügen
            DtFederalState[] federalStates = new DtFederalState[]
            {
                new DtFederalState(){ StateName = "Sachsen"},
                new DtFederalState(){ StateName = "Thüringen"},
                new DtFederalState(){ StateName = "Hessen"},
                new DtFederalState(){ StateName = "Bayern"}
            };

            // Testdaten in Tabelle dt_university einfügen
            List<DtUniversity> universities = new List<DtUniversity>()
            {
                new DtUniversity(){UniversityName = "BA Leipzig", UniversityID = 1 },
                new DtUniversity(){UniversityName = "BA Dresden", UniversityID = 2 },
                new DtUniversity(){UniversityName = "BA Glauchau", UniversityID = 3 },
                new DtUniversity(){UniversityName = "Universität Leipzig", UniversityID = 4 },
                new DtUniversity(){UniversityName = "HTWK Leipzig", UniversityID = 5 }
            };

            foreach (DtFederalState state in federalStates)
            {
                context.Add(state);
            }

            foreach (DtUniversity university in universities)
            {
                context.Add(university);
            }

            context.SaveChanges();
        }
    }
}
