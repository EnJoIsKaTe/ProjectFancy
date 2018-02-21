using System;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using XamPass.Models.DataBaseModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using XamPass.Models;
using XamPass.Controllers;

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
        
        [Test]
        public void ControllerTest1()
        {
            HomeController controller = new HomeController(_context);
        }


        #endregion
    }
}
