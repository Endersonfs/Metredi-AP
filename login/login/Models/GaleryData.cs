using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models
{
    public class GaleryData
    {
        public int ID { get; set; }
        [MaxLength(10),Required(ErrorMessage ="Inserte el nombre de la imagen")] 
        public  string Nombre { get; set; }
        [Required(ErrorMessage = "Inserte la descripcion de la imagen"), Display(Name = "Descripción")]
        public string Description { get; set; }
        [DataType(DataType.ImageUrl)]
        [Display(Name ="Imagen")]
        public string Image { get; set; }
    }
}
