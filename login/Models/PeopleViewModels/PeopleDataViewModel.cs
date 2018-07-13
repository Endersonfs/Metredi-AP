using login.Models.Enumeradores;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using login.Models;
using System.Collections.Generic;

namespace login.Models.PeopleViewModels
{
    public class PeopleDataViewModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        public CargosType Cargo { get; set; }
        
        public SexTypes Sexo { get; set; }
        [Display(Name = "Celular")]
        [Phone]
        public string Celular { get; set; }
        public double Sueldo { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        public string Image { get; set; }

        public IEnumerable<Notifications> Notificaciones { get; set; }

        public IEnumerable<PeopleData> Empleados { get; set; }
      //  public String Empleado { get; set; }

        public  PeopleData peopleData { get; set; }
    }
}
