using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models.Enumeradores
{
    public enum CargosType
    {
        Indefinido,
        CEO,
        Presidente,
        Camarero,
        [Display(Name ="Web Master")]
        Web_Master,
        Decorador,
        [Display(Name = "Servicio de limpieza")]
        Limpieza
    }
}
