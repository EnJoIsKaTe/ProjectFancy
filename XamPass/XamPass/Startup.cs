using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XamPass.Models;
using Microsoft.Extensions.Logging;
using XamPass.Models.DataBaseModels;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace XamPass
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register Database Service
            services.AddDbContext<DataContext>(options
                => options.UseMySql(Configuration.GetConnectionString("DataBaseConnection")));

            // Authentication
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Account/Access");
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/Logout");
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            //services.AddAuthentication("AdminCookieScheme").AddCookie("AdminCookieScheme", options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Admin/Access");
            //    options.LoginPath = new PathString("/Admin/Login");
            //});

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Authentication
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Add LogFile
            loggerFactory.AddFile("Logs/XamPass-{Date}.txt");

            // Add Localization
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("de"),
                new CultureInfo("en")
            };

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("de"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };

            app.UseRequestLocalization(options);
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

        }
    }
}
