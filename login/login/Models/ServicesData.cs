using login.Models.Enumeradores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models
{
    public class ServicesData
    {
        public int ID { get; set; }
        [Display(Name = "Servicio"),Required(ErrorMessage = "Este Campo es Necesario")]
        public string Nombre { get; set; }
        [Display(Name = "Descripción"),Required(ErrorMessage ="Ingresa una Descripcición para el servicio")]
        public string Descripcion { get; set; }
        [Display(Name = "Tipo de Servicio")]
        public IconType icon { get; set; }
    }
}
