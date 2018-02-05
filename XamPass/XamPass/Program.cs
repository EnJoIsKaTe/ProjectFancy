using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XamPass.Models.DataBaseModels;

namespace XamPass
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // try
                //{
                var context = services.GetRequiredService<DataContext>();

                // Solange der DB-Server noch nicht bereit ist sollte diese Methode auskommentiert sein
                //DatabaseTest(context);


                //}
                //catch (Exception ex)
                //{
                //  var logger = services.GetRequiredService<ILogger<Program>>();
                //logger.LogError(ex, "An error occurred while seeding the database.");
                //}
            }

            host.Run();

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

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

            foreach (DtFederalState state in federalStates)
            {
                context.Add(state);
            }

            context.SaveChanges();
        }
    }
}


