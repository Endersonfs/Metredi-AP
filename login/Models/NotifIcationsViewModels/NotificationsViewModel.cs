using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models.NotifIcationsViewModels
{
    public class NotificationsViewModel
    {
        public int ID { get; set; }
        public string Tipo { get; set; }
        [Display(Name = "Sección")]
        public string Section { get; set; }
        //public DateTime Time { get; set; }
        public string Detalle { get; set; }
        [Display(Name ="Fecha")]
        public DateTime Time { get; set; }
        public Notifications notifications { get; set; }
        public IEnumerable<Notifications> Notificaciones { get; set; }
    }
}
