using BerberWeb.DataAccess.Context;
using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BerberWeb.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonelMusaitliksController : Controller
    {
        private readonly BerberWebDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public PersonelMusaitliksController(BerberWebDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var personelMusaitlikler = _context.PersonelMusaitliks
       .Include(p => p.AppUser)
       .ToList();
            return View(personelMusaitlikler);
        }



        public async Task<IActionResult> Create()
        {
            var personelUsers = await _userManager.GetUsersInRoleAsync("personel");

            var personelList = personelUsers
                .Select(u => new
                {
                    FullName = $"{u.FirstName} {u.LastName}",
                    UserId = u.Id
                })
                .ToList();

            ViewBag.PersonelList = personelList;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonelMusaitlik model)
        {
            // Başlangıç ve bitiş saatlerini geçmiş tarih kontrolü
            if (model.BaslangicSaati < DateTime.Now || model.BitisSaati < DateTime.Now)
            {
                ModelState.AddModelError("", "Başlangıç ve bitiş saatleri bugünden itibaren olmalıdır.");

                var personelUsers = await _userManager.GetUsersInRoleAsync("personel");
                var personelList = personelUsers
                    .Select(u => new
                    {
                        FullName = $"{u.FirstName} {u.LastName}",
                        UserId = u.Id
                    })
                    .ToList();

                ViewBag.PersonelList = personelList;
                return View(model);
            }

            model.BaslangicSaati = model.BaslangicSaati.ToUniversalTime();
            model.BitisSaati = model.BitisSaati.ToUniversalTime();

            _context.PersonelMusaitliks.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
