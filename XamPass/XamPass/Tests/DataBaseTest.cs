using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamPass.Models.DataBaseModels;

namespace XamPass.Tests
{
    [TestFixture]
    public class DataBaseTest
    {
        [Test]
        public void TestDataBase()
        {
            var host = Program.BuildWebHost(new string[1] { "" });

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<DataContext>();

                //DatabaseTest(context);
                Assert.NotNull(context);
                Assert.NotNull(services);
            }

        }
    }
}
