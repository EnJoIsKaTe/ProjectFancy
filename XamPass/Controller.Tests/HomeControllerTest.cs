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
            _context.Database.Migrate();

            DBInitialize.DatabaseTest(_context, true);
        }

        [TearDown]
        public void TearDownDatabase()
        {
            _context.Database.EnsureDeleted();
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

        [Test]
        public void CreateAnswerTest()
        {
            HomeController controller = new HomeController(_context);

            ViewModelQuestions vmq = new ViewModelQuestions()
            {
                QuestionId = 1,
                Answer = new DtAnswer() { Content = "Test Answer" }
            };

            controller.CreateAnswer(vmq);

            DtQuestion question = _context.Questions.SingleOrDefault(q => q.QuestionID == 1);

            DtAnswer answer = question.Answers.SingleOrDefault(a => a.Content.Equals("Test Answer"));

            Assert.IsNotNull(answer);
            Assert.AreEqual(question.Answers.Count, 2);
        }

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
