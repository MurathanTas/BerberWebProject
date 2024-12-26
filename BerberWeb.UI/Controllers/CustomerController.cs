using BerberWeb.Entity.Entities;
using BerberWeb.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BerberWeb.UI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public CustomerController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {

            var roleName = "Customer";
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return NotFound($"Rol '{roleName}' bulunamadı.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            return View(usersInRole);
        }

        public IActionResult RegisterCustomer()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterCustomer(CustomerCreateModel model)
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
                    var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("SignIn", "Login");
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
    }
}
