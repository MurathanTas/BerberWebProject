using BerberWeb.DataAccess.Context;
using BerberWeb.Entity.Entities;
using BerberWeb.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BerberWeb.UI.Controllers
{
    public class AdminPersonelRegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly BerberWebDbContext _context;

        public AdminPersonelRegisterController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, BerberWebDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var roleName = "Personel";
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return NotFound($"Rol '{roleName}' bulunamadı.");
            }

            // Personel tablosunu kullanarak ilgili rolü taşıyan kullanıcıları al
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var userIdsInRole = usersInRole.Select(u => u.Id).ToList();

            var sortedPersonel = await _context.Personels
                .Where(p => userIdsInRole.Contains(p.AppUserId))
                .Include(p => p.AppUser)
                .OrderBy(p => p.AppUser.UserName)
                .ToListAsync();

            return View(sortedPersonel);
        }


        public IActionResult CreatePersonnel()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePersonnel(PersonnelCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Personel");

                    if (roleResult.Succeeded)
                    {
                        var personel = new Personel
                        {
                            AppUserId = user.Id
                        };


                        _context.Personels.Add(personel);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }

                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePersonel(int id)
        {
            var personel = await _context.Personels.Include(p => p.AppUser).FirstOrDefaultAsync(p => p.PersonelId == id);

            if (personel == null)
            {
                TempData["ErrorMessage"] = "Silmek istediğiniz personel bulunamadı.";
                return RedirectToAction("Index");
            }

            // Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(personel.AppUserId.ToString());

            if (user == null)
            {
                TempData["ErrorMessage"] = "İlgili kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            // Silme işlemi
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                _context.Personels.Remove(personel);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["ErrorMessage"] = "Silme işlemi sırasında bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = "Personel başarıyla silindi.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Index");
        }
    }
}
