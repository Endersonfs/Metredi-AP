using login.Models.Enumeradores;
using System;
using System.ComponentModel.DataAnnotations;

namespace login.Models
{
    public class PeopleData
    {

        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        public CargosType Cargo { get; set; }
        [Display(Name = "Sexualidad")]
        public SexTypes Sexo { get; set; }
        [Display(Name = "Celular")][Phone]
        public string Celular { get; set; }
        public double Sueldo { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime FechaNacimiento { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }
        [Display(Name = "Perfil")]
        public string Image { get; set; }
    }
}
