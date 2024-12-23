using BerberWeb.Entity.Entities;
using BerberWeb.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BerberWeb.UI.Controllers
{
    public class AdminPersonelRegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminPersonelRegisterController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            var roleName = "Personel";
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return NotFound($"Rol '{roleName}' bulunamadı.");
            }

            // Personel rolüne sahip tüm kullanıcıları alın
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            return View(usersInRole); // Listeyi View'a gönder
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
                // Yeni personel oluşturma işlemi burada yapılacak
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
                    // Personeli rol ekle
                    var roleResult = await _userManager.AddToRoleAsync(user, "Personel");

                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index"); // Başarılı işlem sonrası yönlendirme
                    }

                    // Rol ekleme başarısızsa, hataları ModelState'e ekleyin
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    // Kullanıcı oluşturma başarısızsa, hataları ModelState'e ekleyin
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model); // ModelState invalid ise, tekrar formu döndür
        }
    }
}
