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
using login.Models.GaleryViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

namespace login.Controllers
{
    public class GaleryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IEmpleadosData _empleadosData;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        public GaleryController(ApplicationDbContext context, IEmpleadosData empleadosData, UserManager<ApplicationUser> userManager,
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

        // GET: Galery
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
              return  RedirectToAction(nameof(AccountController.Login),"Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            else { 

               return View(await _context.GaleryData.ToListAsync());
                }
        }

        // GET: Galery/Details/5
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

            var galeryData = await _context.GaleryData
                .SingleOrDefaultAsync(m => m.ID == id);
            if (galeryData == null)
            {
                return NotFound();
            }

            return View(galeryData);
        }

        // GET: Galery/Create
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

        // POST: Galery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Description,Image")] GaleryViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            if (ModelState.IsValid)
            {
                var newPicture = new GaleryData();
                var image = model.Image;

                if(image != null) { 

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                    newPicture.Image = image.FileName;

                }
                else
                {
                    var path = "default.png";
                    newPicture.Image = path;

                }

                newPicture.Nombre = model.Nombre;
                newPicture.Description = model.Description;

                newPicture = _empleadosData.AddPicture(newPicture);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Galery/Edit/5
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

            var galeryData = await _context.GaleryData.SingleOrDefaultAsync(m => m.ID == id);
            if (galeryData == null)
            {
                return NotFound();
            }
            return View(galeryData);
        }

        // POST: Galery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Description,Image")] GaleryData galeryData, IFormFile img)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
                // throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (id != galeryData.ID)
            {
                return NotFound();
            }
            if(img == null)
            {

                if(galeryData != null)
                {

                    _context.Update(galeryData);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));

                }


            }
            else {

            var image = img.FileName;


            //if (ModelState.IsValid)
            //{
                try
                {
                  
                  //  var model = new GaleryViewModel();
                

                    if (image != null)
                    {

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", img.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }
                        galeryData.Image = img.FileName;
                     
                        _context.Update(galeryData);
                        await _context.SaveChangesAsync();
                    }
          
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GaleryDataExists(galeryData.ID))
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
            return View(galeryData);
        }

      

        

        // GET: Galery/Delete/5
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

            var galeryData = await _context.GaleryData
                .SingleOrDefaultAsync(m => m.ID == id);
            if (galeryData == null)
            {
                return NotFound();
            }

            return View(galeryData);
        }




        // POST: Galery/Delete/5
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

            var galeryData = await _context.GaleryData.SingleOrDefaultAsync(m => m.ID == id);
            _context.GaleryData.Remove(galeryData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GaleryDataExists(int id)
        {
            return _context.GaleryData.Any(e => e.ID == id);
        }
    }
}
