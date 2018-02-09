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
                new DtFederalState(){ FederalStateName = "Sachsen"},
                new DtFederalState(){ FederalStateName = "Thüringen"},
                new DtFederalState(){ FederalStateName = "Hessen"},
                new DtFederalState(){ FederalStateName = "Bayern"}
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
                new DtFieldOfStudies(){FieldOfStudiesName = "Informatik", Type = FieldOfStudiesType.Engineering},
                new DtFieldOfStudies(){FieldOfStudiesName = "Bauingenieurwesen", Type = FieldOfStudiesType.Engineering},
                new DtFieldOfStudies(){FieldOfStudiesName = "Betriebswirtschaftslehre", Type = FieldOfStudiesType.Economics},
                new DtFieldOfStudies(){FieldOfStudiesName = "Philosophie", Type = FieldOfStudiesType.SocialScience},
                new DtFieldOfStudies(){FieldOfStudiesName = "Kulturwissenschaften", Type = FieldOfStudiesType.SocialScience},
                new DtFieldOfStudies(){FieldOfStudiesName = "Immobilienwirtschaft", Type = FieldOfStudiesType.Economics},
                new DtFieldOfStudies(){FieldOfStudiesName = "Kunstgeschichte", Type = FieldOfStudiesType.SocialScience},
                new DtFieldOfStudies(){FieldOfStudiesName = "Gesang", Type = FieldOfStudiesType.Arts}
            };

            foreach (DtFieldOfStudies fieldOfStudies in fieldsOfStudies)
            {
                context.Add(fieldOfStudies);
            }

            context.SaveChanges();

            DtSubject[] subjects = new DtSubject[]
            {
                new DtSubject(){SubjectName = "Automaten und formale Sprachen"},
                new DtSubject(){SubjectName = "Berechebarkeit und Komplexität"},
                new DtSubject(){SubjectName = "Personalführung"},
                new DtSubject(){SubjectName = "Stochastik"},
                new DtSubject(){SubjectName = "Lineare Algebra"},
                new DtSubject(){SubjectName = "Netzwerke"},
                new DtSubject(){SubjectName = "Programmieren C++"},

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
                    UpVotes = 77
                }
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
                    FieldOfStudies = context.FieldsOfStudies.SingleOrDefault(f => f.FieldOfStudiesID == 1),
                    Subject = context.Subjects.SingleOrDefault(s => s.SubjectID == 1),
                    SubmissionDate = DateTime.Now,
                    University = context.Universities.SingleOrDefault(u => u.UniversityID == 1),
                    UpVotes = 2},

                new DtQuestion(){Answers = null,
                    Content = "Weitere Frage",
                    FieldOfStudies = context.FieldsOfStudies.SingleOrDefault(u => u.FieldOfStudiesID == 1),
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
