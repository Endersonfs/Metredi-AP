using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using login.Data;
using login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace login.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuarioController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Usuario
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

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Usuario/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Apellido,Address,Cargo,Sexo,Celular,Sueldo,FechaNacimiento,FechaIngreso,Image")] PeopleData peopleData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peopleData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(peopleData);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Apellido,Address,Cargo,Sexo,Celular,Sueldo,FechaNacimiento,FechaIngreso,Image")] PeopleData peopleData)
        {
            if (id != peopleData.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peopleData);
                    await _context.SaveChangesAsync();
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

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peopleData = await _context.PeopleData.SingleOrDefaultAsync(m => m.ID == id);
            _context.PeopleData.Remove(peopleData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeopleDataExists(int id)
        {
            return _context.PeopleData.Any(e => e.ID == id);
        }

        /*public async Task<List<Usuario>> GetUsuario(string id) {
            List<Usuario> usuario = new List<Usuario>();
            return usuario;
        }*/
    }
}
