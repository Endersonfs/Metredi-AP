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
using Microsoft.AspNetCore.Authorization;

namespace login.Controllers
{
    public class PeopleDatasController : Controller
    {
        private ApplicationDbContext _context;
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
        [Authorize(Roles = "Administrador")]
        
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(await _context.PeopleData.ToListAsync());
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

            var peopleData = await _context.PeopleData
                .SingleOrDefaultAsync(m => m.ID == id);
            if (peopleData == null)
            {
                return NotFound();
            }

            return View(peopleData);
        }

        // GET: PeopleDatas/Create
        [Authorize(Roles ="Administrador")]
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
                return View();

            }
        
        }


        // POST: PeopleDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Apellido,Sexo,Address,Cargo,Celular,Sueldo,FechaNacimiento,FechaIngreso,Image")]PeopleDataViewModel model, IFormFile img)
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

                var profileImage = model.Image;

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
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (id == null)
            {
                return NotFound();
            }

            var peopleData = await _context.PeopleData.SingleOrDefaultAsync(m => m.ID == id);
            
            if (peopleData == null)
            {
                return NotFound();
            }
            return View(peopleData);
        }

        // POST: PeopleDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Apellido,Sexo,Address,Cargo,Celular,Sueldo,FechaNacimiento,FechaIngreso,Image")] PeopleData peopleData, IFormFile img)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (id != peopleData.ID)
            {
                return NotFound();
            }

            if (img == null)
            {
     
                if(peopleData != null)
                {

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
                        peopleData.Image = img.FileName;

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

        // GET: PeopleDatas/Delete/5
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

            var peopleData = await _context.PeopleData
                .SingleOrDefaultAsync(m => m.ID == id);
            if (peopleData == null)
            {
                return NotFound();
            }

            return View(peopleData);
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
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var peopleData = await _context.PeopleData.SingleOrDefaultAsync(m => m.ID == id);
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
