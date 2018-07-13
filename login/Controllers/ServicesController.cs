using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using login.Data;
using login.Models;
using Microsoft.AspNetCore.Identity;
using login.Services;
using Microsoft.Extensions.Logging;
using login.Models.ServicesViewModels;

namespace login.Controllers
{
    public class ServicesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private IEmpleadosData _empleadosData;

        public ServicesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            IEmpleadosData empleadosData)
        {
            
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
            _empleadosData = empleadosData;
        }

        // GET: Services
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
                var model = new ServicesViewModel();

                model.Notificaciones = _empleadosData.GetNotifications();
                model.Servicios = _empleadosData.GetAllServices();
                return View(model);
            }
        }

        // GET: Services/Details/5
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
            var model = new ServicesViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.ServicesData = await _context.ServicesData
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Nombre = model.ServicesData.Nombre;
            model.Descripcion = model.ServicesData.Descripcion;
            model.Icon = model.ServicesData.icon;
            if (model.ServicesData == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Services/Create
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
                var model = new ServicesViewModel();
                model.Notificaciones = _empleadosData.GetNotifications();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Descripcion,icon")] ServicesData servicesData)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (ModelState.IsValid)
            {
                var notification = new Notifications();
                TempData["sms"] = "Un nuevo servicio se ha publicado correctamente";
                ViewBag.sms = TempData["sms"];
                notification.Detalle = ViewBag.sms;
                notification.Section = "Servicios";
                notification.Tipo = "check";
                notification.Time = DateTime.Now;
                notification = _empleadosData.AddNotification(notification);
                _context.Add(servicesData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(servicesData);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            var model = new ServicesViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.ServicesData = await _context.ServicesData.SingleOrDefaultAsync(m => m.ID == id);
            model.Nombre = model.ServicesData.Nombre;
            model.Descripcion = model.ServicesData.Descripcion;
            model.Icon = model.ServicesData.icon;

            if (model.ServicesData == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Descripcion,icon")] ServicesData servicesData)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (id != servicesData.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicesData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesDataExists(servicesData.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var notification = new Notifications();
                TempData["sms"] = "Los cambios han sido guardados correctamente";
                ViewBag.sms = TempData["sms"];
                notification.Detalle = ViewBag.sms;
                notification.Section = "Servicios";
                notification.Tipo = "check";

                notification.Time = DateTime.Now;
                notification = _empleadosData.AddNotification(notification);
                return RedirectToAction(nameof(Index));

            }
            return View(servicesData);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            var model = new ServicesViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.ServicesData = await _context.ServicesData
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Nombre = model.ServicesData.Nombre;
            model.Icon = model.ServicesData.icon;
            model.Descripcion = model.ServicesData.Descripcion;
            model.ID = model.ServicesData.ID;
            if (model.ServicesData == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
           
            }
            var servicesData = await _context.ServicesData.SingleOrDefaultAsync(m => m.ID == id);
            var notification = new Notifications();
            TempData["sms"] = "Campo elimiado correctamente";
            ViewBag.sms = TempData["sms"];
            notification.Detalle = ViewBag.sms;
            notification.Section = "Servicios";
            notification.Tipo = "check";
            notification.Time = DateTime.Now;
            notification = _empleadosData.AddNotification(notification);
            _context.ServicesData.Remove(servicesData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicesDataExists(int id)
        {
            return _context.ServicesData.Any(e => e.ID == id);
        }
    }
}
