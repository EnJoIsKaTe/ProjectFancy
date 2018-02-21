using System;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using XamPass.Models.DataBaseModels;
using System.Collections.Generic;
using System.Linq;

namespace Controller.Tests
{
    [TestFixture]
    public class HomeControllerTest
    {
        DataContext _context;


        public HomeControllerTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkMySql()
                .BuildServiceProvider();
            
        }

        [Test]
        public void Test1()
        {
            List<DtQuestion> list = _context.Questions.Where(q => q.QuestionID != 0).ToList();
        }


    }
}
