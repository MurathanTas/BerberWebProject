using BerberWeb.Entity.Entities;
using BerberWeb.UI.Models;
using BerberWeb.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BerberWeb.UI.Controllers
{
    [Authorize]
    public class MyProfileController : Controller
    {
        IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public MyProfileController(IUserService userService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Profile()
        {
            // Kullanıcının ID'sini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);



            if (user != null)
            {
                var email = user.Email;
                var fullName = $"{user.FirstName} {user.LastName}";
                var roles = await _userManager.GetRolesAsync(user);
            }

            return View(user);
        }

        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("SignIn");
            }

            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'si
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return RedirectToAction("SignIn");
                }

                // Kullanıcı bilgilerini güncelle
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi!";
                    return RedirectToAction("Profile");
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
