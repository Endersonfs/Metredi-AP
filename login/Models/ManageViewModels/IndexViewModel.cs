using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }
        [Key]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Número de teléfono")]
        public string PhoneNumber { get; set; }      
        public string StatusMessage { get; set; }

       
        public IEnumerable<Notifications> Notificaciones { get; set; }
        public IEnumerable<PeopleData> Empleados { get; set; }
        public IEnumerable<ChooseUs> Motivos { get; set; }
        public IEnumerable<GaleryData> Galeria { get; set; }
        public IEnumerable<ServicesData> Servicios { get; set; }

        public string IDusuario { get; set; }
        public string IDempleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Address { get; set; }
        public string Celular { get; set; }
        public string Image { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public ApplicationUser Usuario { get; set; }
        public PeopleData Empleado { get; set; }
    }
}
