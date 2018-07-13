using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace login.Models.GaleryViewModels
{
    public class GaleryViewModel
    {
        public int ID { get; set; }
        [MaxLength(10)]
        public string Nombre { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IEnumerable<GaleryData> Pictures { get; set; }
        public IEnumerable<Notifications> Notificaciones { get; set; }
        public GaleryData GaleryData { get; set; }
    }
}
