using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using login.Data;
using login.Models;
using login.Services;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace login
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddSingleton<IFileProvider>(
            //    new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")));


            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            
            services.AddScoped<IEmpleadosData, SqlEmpleadosData>();
            services.AddMvc();

      //google

                    //services.AddAuthentication().AddGoogle(googleOptions =>
                    //{
                    //    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    //    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseGoogleAuthentication(new Microsoft.AspNetCore.Authentication.Google.GoogleOptions()
            //{

            //    ClientId = "126952452080-j28u6fhk95br3ctqajorsamuatngi3ia.apps.googleusercontent.com",
            //    ClientSecret = "2mWoMbnfDlfJm-17oz3435Fq"

            //});
        }
    }
}
