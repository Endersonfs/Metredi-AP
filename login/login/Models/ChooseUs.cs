using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models
{
    public class ChooseUs
    {

        public int ID { get; set; }
        [Display(Name = "Motivo"), Required(ErrorMessage ="En este campo brindarás un motivo por el cual nos elegirían")]
        public string Option { get; set; }
        [Display(Name ="Foto")]
        public string Image { get; set; }
    }
}
