using login.Models.Enumeradores;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using login.Models;


namespace login.Models.PeopleViewModels
{
    public class PeopleDataViewModel
    {
    
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        public CargosType Cargo { get; set; }
        [Display(Name = "Sexualidad")]
        public SexTypes Sexo { get; set; }
        [Display(Name = "Celular")]
        [Phone]
        public string Celular { get; set; }
        public double Sueldo { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }
      
        public IFormFile Image { get; set; }
    }
}
