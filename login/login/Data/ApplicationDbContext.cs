using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using login.Models;
using login.Models.ManageViewModels;

namespace login.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
           // PeopleData.ToString();
        }

        
        public DbSet<login.Models.PeopleData> PeopleData { get; set; }

        
        public  DbSet<login.Models.ServicesData> ServicesData { get; set; }

        
        public DbSet<login.Models.GaleryData> GaleryData { get; set; }

        public string Nombre { get; set; }

        public DbSet<login.Models.ChooseUs> ChooseUs { get; set; }

    }
}
