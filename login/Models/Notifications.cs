using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models
{
    public class Notifications
    {
        public int ID { get; set; }
        //warning, times, check. 
        public string Tipo { get; set; }

        public string Section { get; set; }

        public DateTime Time { get; set; }

        public string Detalle { get; set; }
    }
}
