using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using login.Data;
using login.Models;
using login.Models.PeopleViewModels;
using System.IO;
using login.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace login.Controllers
{
    public class PeopleDatasController : Controller
    {
        private  ApplicationDbContext _context;
        private IEmpleadosData _empleadosData;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public PeopleDatasController(ApplicationDbContext context, IEmpleadosData empleadosData, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
            _empleadosData = empleadosData;
        }

        // GET: PeopleDatas
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            else {
                try
                {
                    ViewBag.sms = TempData["sms"].ToString();
                }
                catch 
                {

                }
                //  var empleado = new PeopleData();

                var model = new PeopleDataViewModel();
              
                    model.Notificaciones = _empleadosData.GetNotifications();
                    model.Empleados = _empleadosData.GetEmpleados();

                return View(model);
               // return View(await _context.PeopleData.ToListAsync());
            }


         
        }

        // GET: PeopleDatas/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (id == null)
            {
                return NotFound();
            }
            var model = new PeopleDataViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.peopleData = await _context.PeopleData
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Nombre = model.peopleData.Nombre;
            model.Sexo = model.peopleData.Sexo;
            model.Address = model.peopleData.Address;
            model.Apellido = model.peopleData.Apellido;
            model.Cargo = model.peopleData.Cargo;
            model.Celular = model.peopleData.Celular;
            model.FechaIngreso = model.peopleData.FechaIngreso;
            model.FechaNacimiento = model.peopleData.FechaNacimiento;
            model.Sexo = model.peopleData.Sexo;
            model.Sueldo = model.peopleData.Sueldo;
            model.Image = model.peopleData.Image;
            if (model.peopleData == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: PeopleDatas/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            else
            {
                var model = new PeopleDataViewModel();
                model.Notificaciones = _empleadosData.GetNotifications();
                return View(model);

            }
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Apellido,Sexo,Address,Cargo,Celular,Sueldo,FechaNacimiento,FechaIngreso,Image")]PeopleData model, IFormFile img)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (ModelState.IsValid)
            {
                var newEmpleado = new PeopleData();

                var profileImage = img;

                if(profileImage != null){ 
           

                var  path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/images",
                            profileImage.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(stream);
                    }
                    newEmpleado.Image = profileImage.FileName;
                }
                else
                {
                    var path = "user.png";
                    newEmpleado.Image = path;
                }              
                newEmpleado.Address = model.Address;
                newEmpleado.Apellido = model.Apellido;
                newEmpleado.Cargo = model.Cargo;
                newEmpleado.Celular = model.Celular;
                newEmpleado.FechaIngreso = model.FechaIngreso;
                newEmpleado.FechaNacimiento = model.FechaNacimiento;
                newEmpleado.Nombre = model.Nombre;
                newEmpleado.Sexo = model.Sexo;
                newEmpleado.Sueldo = model.Sueldo;
                var notification = new Notifications();
               
                TempData["sms"] = "Se agregó al empleado " + model.Nombre  + " exitosamente a la lista de empleados." ;
                ViewBag.sms = TempData["sms"];
                notification.Detalle = ViewBag.sms;
                notification.Section = "Empleados";
                notification.Tipo = "check";
                notification.Time = DateTime.Now;
                notification = _empleadosData.AddNotification(notification);
                newEmpleado = _empleadosData.Add(newEmpleado);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
           
        }

        // GET: PeopleDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
           
            }
            if (id == null)
            {
                return NotFound();
            }

            var model = new PeopleDataViewModel();

            model.Notificaciones = _empleadosData.GetNotifications();            
            model.peopleData = await _context.PeopleData.SingleOrDefaultAsync(m => m.ID == id);
            model.Nombre = model.peopleData.Nombre;
            model.Sexo = model.peopleData.Sexo;
            model.Address = model.peopleData.Address;
            model.Apellido = model.peopleData.Apellido;
            model.Cargo = model.peopleData.Cargo;
            model.Celular = model.peopleData.Celular;
            model.FechaIngreso = model.peopleData.FechaIngreso;
            model.FechaNacimiento = model.peopleData.FechaNacimiento;
            model.Sexo = model.peopleData.Sexo;
            model.Sueldo = model.peopleData.Sueldo;
            model.Image = model.peopleData.Image;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Apellido,Sexo,Address,Cargo,Celular,Sueldo,FechaNacimiento,FechaIngreso,Image")] PeopleData peopleData, IFormFile img)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            if (id != peopleData.ID)
            {
                return NotFound();
            }

            if (img == null)
            {
     
                if(peopleData != null)
                {
                    var notification = new Notifications();
                    TempData["sms"] = "Los cambios se han guardado con satisfacción!";
                    ViewBag.sms = TempData["sms"];
                    notification.Detalle = ViewBag.sms;
                    notification.Section = "Empleados";
                    notification.Tipo = "check";
                    notification.Time = DateTime.Now;
                    notification = _empleadosData.AddNotification(notification);
                    _context.Update(peopleData);
                     await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else {

                var image = img.FileName;

                //if (ModelState.IsValid)
                //{
                try
                {

                    if (image != null)
                    {

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", img.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }
                        var notification = new Notifications();
                        peopleData.Image = img.FileName;
                        TempData["sms"] = "Los cambios se han realizado satisfactoriamente!";
                        ViewBag.sms = TempData["sms"];
                        notification.Detalle = ViewBag.sms;
                        notification.Section = "Empleados";
                        notification.Tipo = "check";
                        notification.Time = DateTime.Now;
                        notification = _empleadosData.AddNotification(notification);
                        _context.Update(peopleData);

                        await _context.SaveChangesAsync();
                    }
              
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!PeopleDataExists(peopleData.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(peopleData);
        }

    
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            if (id == null)
            {
                return NotFound();
            }
            var model = new PeopleDataViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();

            model.peopleData = await _context.PeopleData
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Nombre = model.peopleData.Nombre;
            model.Sexo = model.peopleData.Sexo;
            model.Address = model.peopleData.Address;
            model.Apellido = model.peopleData.Apellido;
            model.Cargo = model.peopleData.Cargo;
            model.Celular = model.peopleData.Celular;
            model.FechaIngreso = model.peopleData.FechaIngreso;
            model.FechaNacimiento = model.peopleData.FechaNacimiento;
            model.Sexo = model.peopleData.Sexo;
            model.Sueldo = model.peopleData.Sueldo;
            model.Image = model.peopleData.Image;
            model.ID = model.peopleData.ID;
            if (model.peopleData == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: PeopleDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
              
            }
            var notification = new Notifications();
            var peopleData = await _context.PeopleData.SingleOrDefaultAsync(m => m.ID == id);
            TempData["sms"] = "Campo eliminado correctamente!";
            ViewBag.sms = TempData["sms"];
            notification.Detalle = ViewBag.sms;
            notification.Section = "Empleados";
            notification.Tipo = "check";
            notification.Time = DateTime.Now;
            notification = _empleadosData.AddNotification(notification);
            _context.PeopleData.Remove(peopleData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeopleDataExists(int id)
        {
            return _context.PeopleData.Any(e => e.ID == id);
        }
    }
}
