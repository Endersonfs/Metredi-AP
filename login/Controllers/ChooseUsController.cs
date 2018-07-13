using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using login.Data;
using login.Models;
using login.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using login.Models.ChooseUsViewModels;

namespace login.Controllers
{
    public class ChooseUsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private IEmpleadosData _empleadosData;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        public ChooseUsController(ApplicationDbContext context, IEmpleadosData empleadosData, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<ManageController> logger,
            UrlEncoder urlEncoder)
        {
            _context = context;
            _empleadosData = empleadosData;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        // GET: ChooseUs
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
               
            }
     
            else
            {
                try
                {
                    ViewBag.sms = TempData["sms"].ToString();
                }
                catch 
                {

                    
                }

                var model = new ChooseUsViewModel();
                model.Notificaciones = _empleadosData.GetNotifications();
                model.Motivos = _empleadosData.GetAllTheReasons();
                return View(model);
                
            }
           
        }

        // GET: ChooseUs/Details/5
        public async Task<IActionResult> Details(int? id)
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
            var model = new ChooseUsViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.ChooseUs = await _context.ChooseUs
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Image = model.ChooseUs.Image;
            model.Option = model.ChooseUs.Option;
            if (model.ChooseUs == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: ChooseUs/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");

            }
            var model = new ChooseUsViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            return View(model);
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Option,Image")] ChooseUs chooseUs, IFormFile Img)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");

            }

            if (ModelState.IsValid)
            {

                if (Img != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Img.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Img.CopyToAsync(stream);
                    }
                    chooseUs.Image = Img.FileName;

                }
                else
                {
                    var path = "default.png";
                    chooseUs.Image = path;
                }
                var notification = new Notifications();

                TempData["sms"] = "Campo añadido correctamente";
                ViewBag.sms = TempData["sms"];
                notification.Detalle = ViewBag.sms;
                notification.Section = "Razones para elegirnos";
                notification.Tipo = "check";
                notification.Time = DateTime.Now;
                notification = _empleadosData.AddNotification(notification);

                chooseUs = _empleadosData.AddReason(chooseUs);
                return RedirectToAction(nameof(Index));

            }
            return View(chooseUs);
        }

        // GET: ChooseUs/Edit/5
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
            var model = new ChooseUsViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.ChooseUs = await _context.ChooseUs.SingleOrDefaultAsync(m => m.ID == id);
            model.Image = model.ChooseUs.Image;
            model.Option = model.ChooseUs.Option;
            if (model.ChooseUs == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Option,Image")] ChooseUs chooseUs, IFormFile Img)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");

            }
            if (id != chooseUs.ID)
            {
                return NotFound();
            }

            if (Img == null)
            {

                if (chooseUs != null)
                {
                    var notification = new Notifications();
                    TempData["sms"] = "Se han guardado los cambios correctamente";
                    ViewBag.sms = TempData["sms"];
                    notification.Detalle = ViewBag.sms;
                    notification.Section = "Motivos para elegirnos";
                    notification.Tipo = "check";
                    notification.Time = DateTime.Now;
                    notification = _empleadosData.AddNotification(notification);
                    _context.Update(chooseUs);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));

                }


            }
            else
            {

                var image = Img.FileName;

                try
                {

                    if (image != null)
                    {

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Img.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await Img.CopyToAsync(stream);
                        }
                        var notification = new Notifications();
                        chooseUs.Image = Img.FileName;
                        TempData["sms"] = "Se han guardado los cambios correctamente";
                        ViewBag.sms = TempData["sms"];
                        notification.Detalle = ViewBag.sms;
                        notification.Section = "Motivos para elegirnos";
                        notification.Tipo = "check";
                        notification.Time = DateTime.Now;
                        notification = _empleadosData.AddNotification(notification);

                        _context.Update(chooseUs);
                        await _context.SaveChangesAsync();
                    }
                }
         
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChooseUsExists(chooseUs.ID))
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
            return View(chooseUs);
        }

        // GET: ChooseUs/Delete/5
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
            var model = new ChooseUsViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.ChooseUs = await _context.ChooseUs
                .SingleOrDefaultAsync(m => m.ID == id);
            model.ID = model.ChooseUs.ID;
            model.Image = model.ChooseUs.Image;
            model.Option = model.ChooseUs.Option;

            if (model.ChooseUs == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: ChooseUs/Delete/5
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
            var chooseUs = await _context.ChooseUs.SingleOrDefaultAsync(m => m.ID == id);
            TempData["sms"] = "Campo eliminado correctamente";
            ViewBag.sms = TempData["sms"];
            notification.Detalle = ViewBag.sms;
            notification.Section = "Motivos para elegirnos";
            notification.Tipo = "check";
            notification.Time = DateTime.Now;
            notification = _empleadosData.AddNotification(notification);
            _context.ChooseUs.Remove(chooseUs);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool ChooseUsExists(int id)
        {
            return _context.ChooseUs.Any(e => e.ID == id);
        }
    }
}
