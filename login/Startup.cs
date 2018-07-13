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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
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
        //public void configure
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void  Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
            CreateRoles(serviceProvider);

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

                    
        }


        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var role = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var user = serviceProvider.GetRequiredService<RoleManager<ApplicationUser>>();
            string[] rolesNames = { "Admin", "User" };
            IdentityResult result;
            foreach(var rolesName in rolesNames)
            {
                var roleExist = await role.RoleExistsAsync(rolesName);
                if (!roleExist)
                {
                    result = await role.CreateAsync(new IdentityRole(rolesName));
                }
            }
        }

    }
}
