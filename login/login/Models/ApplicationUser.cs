using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace login.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
   
        //public string Nombre { get; set; }
        //public string Apellido { get; set; }
        //public string Address { get; set; }
        //public string Cargo { get; set; }
        //public string Celular { get; set; }
        //public double Sueldo { get; set; }
        //public DateTime FechaNacimiento { get; set; }
        //public DateTime FechaIngreso { get; set; }
        //public IFormFile Image { get; set; }
    }
}
