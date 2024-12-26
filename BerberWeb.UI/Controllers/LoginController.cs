using BerberWeb.Entity.Entities;
using BerberWeb.UI.Models;
using BerberWeb.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BerberWeb.UI.Controllers
{
    public class LoginController : Controller
    {
        IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;



        public LoginController(IUserService userService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userRole = await _userService.LoginAsync(model);

                if (userRole == "Admin")
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    TempData["WelcomeMessage"] = $"Hoş geldiniz, {user.FirstName}!";

                    return RedirectToAction("Profile", "MyProfile");
                }

                if (userRole == "Personel")
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    TempData["WelcomeMessage"] = $"Hoş geldiniz, {user.FirstName}!";

                    return RedirectToAction("Profile", "MyProfile");
                }

                if (userRole == "Customer")
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    TempData["WelcomeMessage"] = $"Hoş geldiniz, {user.FirstName}!";

                    return RedirectToAction("Profile", "MyProfile");
                }
                else
                {
                    ModelState.AddModelError("", "Email veya Şifre Hatalı");
                }
            }

            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Login");
        }
    }
}
