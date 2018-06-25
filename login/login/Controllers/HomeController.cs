using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using login.Models;
using login.Models.PeopleViewModels;
using login.Services;

namespace login.Controllers
{
    public class HomeController : Controller
    {
        private IEmpleadosData _EmpleadosData;
        public HomeController(IEmpleadosData empleadosData)
        {
            _EmpleadosData = empleadosData; 
        }


        public IActionResult Index()
        {
            var model = new EmpleadosViewModel();
            

            model.Empleados = _EmpleadosData.GetAll();

            model.Razones = _EmpleadosData.GetAllTheReasons();

            model.Servicios = _EmpleadosData.GetAllServices();

            model.Fotos = _EmpleadosData.GetAllPictures();

            return View(model);

           
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
