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
        [MaxLength(10)]
        public string Nombre { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
