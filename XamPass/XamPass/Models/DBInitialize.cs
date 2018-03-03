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
        public static void DatabaseTest(DataContext context, bool createNewDatabase)
        {
            if (createNewDatabase)
            {
                // Löscht die Datenbank falls vorhanden
                context.Database.EnsureDeleted();

                // Erstellt die Datenbank neu auf Grundlage der Model-Klassen
                context.Database.EnsureCreated();
            }

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

            // Testdaten in die Tabelle dt_federal_state einfügen
            DtFederalState[] federalStates = new DtFederalState[]
            {
                new DtFederalState(){ FederalStateName = "Baden-Württemberg"},
                new DtFederalState(){ FederalStateName = "Bayern"},
                new DtFederalState(){ FederalStateName = "Berlin"},
                new DtFederalState(){ FederalStateName = "Brandenburg"},
                new DtFederalState(){ FederalStateName = "Bremen"},
                new DtFederalState(){ FederalStateName = "Hamburg"},
                new DtFederalState(){ FederalStateName = "Hessen"},
                new DtFederalState(){ FederalStateName = "Mecklenburg-Vorpommern"},
                new DtFederalState(){ FederalStateName = "Niedersachsen"},
                new DtFederalState(){ FederalStateName = "Nordrhein-Westfalen"},
                new DtFederalState(){ FederalStateName = "Rheinland-Pfalz"},
                new DtFederalState(){ FederalStateName = "Saarland"},
                new DtFederalState(){ FederalStateName = "Sachsen"},
                new DtFederalState(){ FederalStateName = "Sachsen-Anhalt"},
                new DtFederalState(){ FederalStateName = "Schleswig-Holstein"},
                new DtFederalState(){ FederalStateName = "Thüringen"}
            };

            foreach(DtFederalState state in federalStates)
            {
                context.Add(state);
            }

            context.SaveChanges();

            // Testdaten in Tabelle dt_university einfügen
            List<DtUniversity> universities = new List<DtUniversity>()
            {
                new DtUniversity(){UniversityName = "BA Leipzig", CountryID = 1, FederalStateID = 13 },
                new DtUniversity(){UniversityName = "BA Dresden",  CountryID = 1, FederalStateID = 13},
                new DtUniversity(){UniversityName = "BA Glauchau", CountryID = 1, FederalStateID = 13},
                new DtUniversity(){UniversityName = "Universität Leipzig", CountryID = 1, FederalStateID = 13},
                new DtUniversity(){UniversityName = "HTWK Leipzig", CountryID = 1, FederalStateID = 13},
                new DtUniversity(){UniversityName = "Universität Jena", CountryID = 1, FederalStateID = 16},
                new DtUniversity(){UniversityName = "HfM Weimar", CountryID = 1, FederalStateID = 16},
                new DtUniversity(){UniversityName = "Universität Kassel", CountryID = 1, FederalStateID = 7},
                new DtUniversity(){UniversityName = "Universität Würzburg", CountryID = 1, FederalStateID = 2}
            };

            foreach (DtUniversity university in universities)
            {
                context.Add(university);
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
                new DtSubject(){SubjectName = "Berechenbarkeit und Komplexität"},
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
                new DtAnswer(){ /*QuestionId = 1,*/
                    SubmissionDate = DateTime.Now,
                    Content = "Ich weiß doch nicht wie eine Turing Maschine aussieht!!!11!",
                    UpVotes = 77
                },
                new DtAnswer(){ /*QuestionId = 3,*/
                    SubmissionDate = DateTime.Now,
                    Content = "Blau! Nein rot!",
                    UpVotes = 2
                }
          };

            foreach (DtAnswer answer in answers)
            {
                context.Add(answer);
            }

            context.SaveChanges();

            List<DtQuestion> questions = new List<DtQuestion>()
            {
                new DtQuestion(){Answers = context.Answers.Where(a => a.AnswerID == 1).ToList(),
                    Content = "Konstruieren Sie eine Turing Maschine",
                    FieldOfStudies = context.FieldsOfStudies.FirstOrDefault(f => f.FieldOfStudiesID == 1),
                    Subject = context.Subjects.FirstOrDefault(s => s.SubjectID == 1),
                    SubmissionDate = DateTime.Now,
                    University = context.Universities.FirstOrDefault(u => u.UniversityID == 1),
                    UpVotes = 2},

                new DtQuestion(){Answers = null,
                    Content = "Was soll das alles?",
                    FieldOfStudies = context.FieldsOfStudies.FirstOrDefault(u => u.FieldOfStudiesID == 1),
                    Subject = context.Subjects.FirstOrDefault(u => u.SubjectID == 1),
                    SubmissionDate = DateTime.Now,
                    University = context.Universities.FirstOrDefault(u => u.UniversityID == 2),
                    UpVotes = 0},

                 new DtQuestion(){Answers = context.Answers.Where(a => a.AnswerID == 2).ToList(),
                    Content = "Was ist deine Lieblingsfarbe?",
                    FieldOfStudies = context.FieldsOfStudies.FirstOrDefault(u => u.FieldOfStudiesID == 1),
                    Subject = context.Subjects.FirstOrDefault(u => u.SubjectID == 1),
                    SubmissionDate = DateTime.Now,
                    University = context.Universities.FirstOrDefault(u => u.UniversityID == 3),
                    UpVotes = 0}
            };

            // weitere Fragen eintragen
            for (int i = 1; i < 6; i++)
            {
                var question = new DtQuestion()
                {
                    Answers = null,
                    Content = String.Format("Weitere Frage {0}", i),
                    FieldOfStudies = context.FieldsOfStudies.FirstOrDefault(u => u.FieldOfStudiesID == i),
                    Subject = context.Subjects.FirstOrDefault(u => u.SubjectID == i),
                    SubmissionDate = DateTime.Now,
                    University = context.Universities.FirstOrDefault(u => u.UniversityID == i),
                    UpVotes = 0
                };

                questions.Add(question);
            }

            foreach (DtQuestion question in questions)
            {
                context.Add(question);
            }

            context.SaveChanges();
          
        }
    }
}
