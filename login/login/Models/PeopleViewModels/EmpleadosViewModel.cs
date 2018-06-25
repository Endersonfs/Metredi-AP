
using System.Collections.Generic;
using login.Models;

namespace login.Models.PeopleViewModels
{
    public class EmpleadosViewModel
    {

        public IEnumerable<ServicesData> Servicios { get; set; }

        public IEnumerable<PeopleData> Empleados { get; set; }
         
        public IEnumerable<ChooseUs> Razones { get; set; } 
        public IEnumerable<GaleryData> Fotos { get; set; }
    }
}
