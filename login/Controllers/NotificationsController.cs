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
using login.Models.NotifIcationsViewModels;

namespace login.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IEmpleadosData _empleadosData;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        public NotificationsController(ApplicationDbContext context, IEmpleadosData empleadosData, UserManager<ApplicationUser> userManager,
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

        // GET: Notifications
        public async Task<IActionResult> Index()
        { 
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            var model = new NotificationsViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            
            
            return View(model);
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = new NotificationsViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            model.notifications = await _context.Notifications
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Detalle = model.notifications.Detalle;
            model.Section = model.notifications.Section;
            model.Time = model.notifications.Time;
            model.Tipo = model.notifications.Tipo;
            model.ID = model.notifications.ID;
            if (model.notifications == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Tipo,Detalle")] Notifications notifications)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notifications);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notifications);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifications = await _context.Notifications.SingleOrDefaultAsync(m => m.ID == id);
            if (notifications == null)
            {
                return NotFound();
            }
            return View(notifications);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Tipo,Detalle")] Notifications notifications)
        {
            if (id != notifications.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notifications);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationsExists(notifications.ID))
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
            return View(notifications);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = new NotificationsViewModel();
            model.Notificaciones = _empleadosData.GetNotifications();
            
            model.notifications = await _context.Notifications
                .SingleOrDefaultAsync(m => m.ID == id);
            model.Time = model.notifications.Time;
            model.Section = model.notifications.Section;
            model.Tipo = model.notifications.Tipo;
            model.Detalle = model.notifications.Detalle;
            if (model.notifications == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notifications = await _context.Notifications.SingleOrDefaultAsync(m => m.ID == id);
            _context.Notifications.Remove(notifications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationsExists(int id)
        {
            return _context.Notifications.Any(e => e.ID == id);
        }
    }
}
