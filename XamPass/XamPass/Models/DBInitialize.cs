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


            DtCountry[] countries = new DtCountry[]
            {
                new DtCountry(){CountryName = "Deutschland"},
                new DtCountry(){CountryName = "Österreich"},
                new DtCountry(){CountryName = "Schweiz"}
            };

            foreach (DtCountry country in countries)
            {
                context.Add(country);
            }

            context.SaveChanges();

            DtFieldOfStudies[] fieldsOfStudies = new DtFieldOfStudies[]
            {
                new DtFieldOfStudies(){Name = "Informatik", Type = FieldOfStudiesType.Engineering},
                new DtFieldOfStudies(){Name = "Bauingenieurwesen", Type = FieldOfStudiesType.Engineering},
                new DtFieldOfStudies(){Name = "Betriebswirtschaftslehre", Type = FieldOfStudiesType.Economics},
                new DtFieldOfStudies(){Name = "Philosophie", Type = FieldOfStudiesType.SocialScience},
                new DtFieldOfStudies(){Name = "Kulturwissenschaften", Type = FieldOfStudiesType.SocialScience},
                new DtFieldOfStudies(){Name = "Immobilienwirtschaft", Type = FieldOfStudiesType.Economics},
                new DtFieldOfStudies(){Name = "Kunstgeschichte", Type = FieldOfStudiesType.SocialScience},
                new DtFieldOfStudies(){Name = "Gesang", Type = FieldOfStudiesType.Arts}
            };

            foreach (DtFieldOfStudies fieldOfStudies in fieldsOfStudies)
            {
                context.Add(fieldOfStudies);
            }

            context.SaveChanges();

            DtSubject[] subjects = new DtSubject[]
            {
                new DtSubject(){Name = "Automaten und formale Sprachen"},
                new DtSubject(){Name = "Berechebarkeit und Komplexität"},
                new DtSubject(){Name = "Personalführung"},
                new DtSubject(){Name = "Stochastik"},
                new DtSubject(){Name = "Lineare Algebra"},
                new DtSubject(){Name = "Netzwerke"},
                new DtSubject(){Name = "Programmieren C++"},

            };

            foreach (DtSubject subject in subjects)
            {
                context.Add(subject);
            }

            context.SaveChanges();

            DtAnswer[] answers = new DtAnswer[]
            {
                new DtAnswer(){SubmissionDate = DateTime.Now,
                    Content = "Ich weiß doch nicht wie eine Turing Maschine aussieht!!!11!",
                    UpVotes = 77}
            };

            foreach (DtAnswer answer in answers)
            {
                context.Add(answer);
            }

            context.SaveChanges();

            DtQuestion[] questions = new DtQuestion[]
            {
                new DtQuestion(){Answers = context.Answers.Where(a => a.AnswerID == 1).ToList(),
                    Content = "Konstruieren Sie eine Turing Maschine",
                    FieldOfStudies = context.FieldsOfStidies.SingleOrDefault(f => f.FieldOfStudiesID == 1),
                    Subject = context.Subjects.SingleOrDefault(s => s.SubjectID == 1),
                    SubmissionDate = DateTime.Now,
                    University = context.Universities.SingleOrDefault(u => u.UniversityID == 1),
                    UpVotes = 2},

                new DtQuestion(){Answers = null,
                    Content = "Weitere Frage",
                    FieldOfStudies = context.FieldsOfStidies.SingleOrDefault(u => u.FieldOfStudiesID == 1),
                    Subject = context.Subjects.SingleOrDefault(u => u.SubjectID == 1),
                    SubmissionDate = DateTime.Now,
                    University = context.Universities.SingleOrDefault(u => u.UniversityID == 1),
                    UpVotes = 0}
            };

            foreach (DtQuestion question in questions)
            {
                context.Add(question);
            }

            context.SaveChanges();
        }
    }
}
