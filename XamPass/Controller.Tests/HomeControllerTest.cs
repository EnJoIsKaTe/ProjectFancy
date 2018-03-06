using System;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using XamPass.Models.DataBaseModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using XamPass.Models;
using XamPass.Controllers;
using Microsoft.AspNetCore.Mvc;
using XamPass.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Controller.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        DataContext _context;

        [SetUp]
        public void SetUpDatabase()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkMySql()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DataContext>();

            builder.UseMySql($"server = localhost; userid=dev; pwd=geheim; port=3306; database=Test_XamPassDatabase; sslmode=none");

            _context = new DataContext(builder.Options);
            //_context.Database.Migrate();

            DatabaseTest(_context, true);
        }
        /// <summary>
        /// Set up Test Database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="createNewDatabase"></param>
        public static void DatabaseTest(DataContext context, bool createNewDatabase)
        {
            if (createNewDatabase)
            {
                // Deletes the Database if it exists
                context.Database.EnsureDeleted();

                // Creates the Database if does not exist yet
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

            foreach (DtFederalState state in federalStates)
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

        [TearDown]
        public void TearDownDatabase()
        {
            // TODO Benjamin: mache das noch ordentlich

            //_context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region DatabaseTests
        [Test]
        public void Created()
        {
            Assert.IsTrue(_context.Database.IsMySql());
            List<DtUniversity> unis = _context.Universities.ToList();
            Assert.AreNotEqual(unis.Count, 0);
            Assert.AreEqual(unis.First().UniversityName, "BA Leipzig");

            List<DtQuestion> questions = _context.Questions.ToList();
            Assert.AreNotEqual(questions.Count, 0);
        }

        [Test]
        public void AddUniversity()
        {
            DtUniversity university = new DtUniversity()
            {
                UniversityName = "TestUniversity",
                CountryID = 1,
                FederalStateID = 1
            };

            _context.Add(university);

            _context.SaveChanges();

            DtUniversity loadedUniversity = _context.Universities.SingleOrDefault(u => u.UniversityName.Equals(university.UniversityName));

            Assert.NotNull(loadedUniversity);
            Assert.AreEqual(loadedUniversity.CountryID, university.CountryID);
        }

        #endregion

        #region SetFilterMethodTests

        [Test]
        public void FillAllDropdownsTest()
        {
            HomeController homeController = new HomeController(_context, null);

            ViewModelSearch viewModelSearch = new ViewModelSearch();

            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            Assert.AreEqual(9, viewModelSearch.Universities.Count);
            Assert.AreEqual(8, viewModelSearch.FieldsOfStudies.Count);
            Assert.AreEqual(7, viewModelSearch.Subjects.Count);
            Assert.AreEqual(16, viewModelSearch.FederalStates.Count);
        }

        [Test]
        public void SetFilterForUniversitiesTest()
        {
            HomeController homeController = new HomeController(_context, null);

            ViewModelSearch viewModelSearch = new ViewModelSearch();

            // First Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.FieldOfStudiesId = 1;
            viewModelSearch.SubjectId = 1;

            List<DtQuestion> questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForUniversities(questions, viewModelSearch);

            Assert.AreEqual(3, viewModelSearch.Universities.Count);
            Assert.AreEqual("BA Dresden", viewModelSearch.Universities.First().Text);
            Assert.AreEqual("BA Leipzig", viewModelSearch.Universities.Last().Text);



            // Second Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.FieldOfStudiesId = 1;
            viewModelSearch.SubjectId = 2;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForUniversities(questions, viewModelSearch);

            Assert.AreEqual(9, viewModelSearch.Universities.Count);
            Assert.AreEqual("BA Dresden", viewModelSearch.Universities.First().Text);
            Assert.AreEqual("Universität Würzburg", viewModelSearch.Universities.Last().Text);
        }

        [Test]
        public void SetFilterForFieldsOfStudiesTest()
        {
            HomeController homeController = new HomeController(_context, null);

            ViewModelSearch viewModelSearch = new ViewModelSearch();

            // First Test
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 1;
            viewModelSearch.SubjectId = 1;

            List<DtQuestion> questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForFieldsOfStudies(questions, viewModelSearch);

            Assert.AreEqual(1, viewModelSearch.FieldsOfStudies.Count);
            Assert.AreEqual("Informatik", viewModelSearch.FieldsOfStudies.First().Text);
            Assert.AreEqual("Informatik", viewModelSearch.FieldsOfStudies.Last().Text);



            // Second Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 2;
            viewModelSearch.SubjectId = null;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForFieldsOfStudies(questions, viewModelSearch);

            Assert.AreEqual(2, viewModelSearch.FieldsOfStudies.Count);
            Assert.AreEqual("Bauingenieurwesen", viewModelSearch.FieldsOfStudies.First().Text);
            Assert.AreEqual("Informatik", viewModelSearch.FieldsOfStudies.Last().Text);



            // Third Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = null;
            viewModelSearch.SubjectId = 2;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForFieldsOfStudies(questions, viewModelSearch);

            Assert.AreEqual(1, viewModelSearch.FieldsOfStudies.Count);
            Assert.AreEqual("Bauingenieurwesen", viewModelSearch.FieldsOfStudies.First().Text);
            Assert.AreEqual("Bauingenieurwesen", viewModelSearch.FieldsOfStudies.Last().Text);


            // Fourth Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 5;
            viewModelSearch.SubjectId = 1;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForFieldsOfStudies(questions, viewModelSearch);

            Assert.AreEqual(8, viewModelSearch.FieldsOfStudies.Count);
            Assert.AreEqual("Bauingenieurwesen", viewModelSearch.FieldsOfStudies.First().Text);
            Assert.AreEqual("Philosophie", viewModelSearch.FieldsOfStudies.Last().Text);
        }

        [Test]
        public void SetFilterForSubjectsTest()
        {
            HomeController homeController = new HomeController(_context, null);

            ViewModelSearch viewModelSearch = new ViewModelSearch();

            // First Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 1;
            viewModelSearch.FieldOfStudiesId = 1;

            List<DtQuestion> questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForSubjects(questions, viewModelSearch);

            Assert.AreEqual(1, viewModelSearch.Subjects.Count);
            Assert.AreEqual("Automaten und formale Sprachen", viewModelSearch.Subjects.First().Text);
            Assert.AreEqual("Automaten und formale Sprachen", viewModelSearch.Subjects.Last().Text);


            // Second Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 1;
            viewModelSearch.FieldOfStudiesId = null;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForSubjects(questions, viewModelSearch);

            Assert.AreEqual(1, viewModelSearch.Subjects.Count);
            Assert.AreEqual("Automaten und formale Sprachen", viewModelSearch.Subjects.First().Text);
            Assert.AreEqual("Automaten und formale Sprachen", viewModelSearch.Subjects.Last().Text);



            // Third Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 2;
            viewModelSearch.FieldOfStudiesId = null;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForSubjects(questions, viewModelSearch);

            Assert.AreEqual(2, viewModelSearch.Subjects.Count);
            Assert.AreEqual("Automaten und formale Sprachen", viewModelSearch.Subjects.First().Text);
            Assert.AreEqual("Berechenbarkeit und Komplexität", viewModelSearch.Subjects.Last().Text);

            // Fourth Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 1;
            viewModelSearch.FieldOfStudiesId = 5;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForSubjects(questions, viewModelSearch);

            Assert.AreEqual(7, viewModelSearch.Subjects.Count);
            Assert.AreEqual("Automaten und formale Sprachen", viewModelSearch.Subjects.First().Text);
            Assert.AreEqual("Stochastik", viewModelSearch.Subjects.Last().Text);

            // Fifth Test
            viewModelSearch = new ViewModelSearch();
            viewModelSearch = homeController.FillAllDropdowns(viewModelSearch);

            viewModelSearch.UniversityId = 1;

            questions = _context.Questions
              .Where(q => (viewModelSearch.FieldOfStudiesId != null ? q.FieldOfStudiesID == viewModelSearch.FieldOfStudiesId : q.FieldOfStudiesID != 0))
              .Where(q => (viewModelSearch.SubjectId != null ? q.SubjectID == viewModelSearch.SubjectId : q.SubjectID != 0))
              .Where(q => (viewModelSearch.UniversityId != null ? q.UniversityID == viewModelSearch.UniversityId : q.UniversityID != 0))
              .Where(q => (viewModelSearch.FederalStateId != null ? q.University.FederalStateID == viewModelSearch.FederalStateId : q.University.FederalStateID != 0))
              .ToList();

            viewModelSearch = homeController.SetFilterForSubjects(questions, viewModelSearch);

            Assert.AreEqual(1, viewModelSearch.Subjects.Count);
            Assert.AreEqual("Automaten und formale Sprachen", viewModelSearch.Subjects.First().Text);
        }


        #endregion

        #region MethodsTests

        //[Test]
        //public void CreateNewEntryTest()
        //{
        //    HomeController controller = new HomeController(_context);

        //    string testTitle = "Test Title";

        //    ViewModelSearch viewModelSearch = new ViewModelSearch()
        //    {
        //        FederalStateId = 1,
        //        FieldOfStudiesId = 1,
        //        QuestionContent = "Test Content",
        //        SubjectId = 1,
        //        QuestionTitle = testTitle,
        //        UniversityId = 1
        //    };

        //    controller.CreateNewEntry(viewModelSearch);

        //    DtQuestion question = _context.Questions.SingleOrDefault(q => q.Title.Equals(testTitle));

        //    Assert.IsNotNull(question);
        //    Assert.AreEqual(question.FieldOfStudiesID, 1);
        //    Assert.AreEqual(question.SubjectID, 1);
        //    Assert.AreEqual(question.UniversityID, 1);
        //}

        //[Test]
        //public void CreateAnswerTest()
        //{
        //    HomeController controller = new HomeController(_context);

        //    ViewModelQuestions vmq = new ViewModelQuestions()
        //    {
        //        QuestionId = 1,
        //        Answer = new DtAnswer() { Content = "Test Answer" }
        //    };

        //    controller.CreateAnswer(vmq);

        //    DtQuestion question = _context.Questions.SingleOrDefault(q => q.QuestionID == 1);

        //    DtAnswer answer = question.Answers.SingleOrDefault(a => a.Content.Equals("Test Answer"));

        //    Assert.IsNotNull(answer);
        //    Assert.AreEqual(question.Answers.Count, 2);
        //}

        //[Test]
        //public void ViewQuestionTest()
        //{
        //    HomeController controller = new HomeController(_context);

        //    ViewModelQuestions vmq = new ViewModelQuestions()
        //    {
        //        QuestionId = 1
        //    };

        //    controller.ViewQuestion(vmq);

        //    Assert.IsNotNull(vmq.Question);
        //    Assert.IsNotNull(vmq.FieldOfStudies);
        //    Assert.IsNotNull(vmq.Subject);
        //    Assert.IsNotNull(vmq.University);
        //    Assert.IsNotNull(vmq.FederalState);
        //    Assert.IsNotNull(vmq.Answers);

        //    Assert.AreEqual(vmq.QuestionId, 1);            
        //}

        #endregion
    }
}
