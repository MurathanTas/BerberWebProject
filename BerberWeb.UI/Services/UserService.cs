using BerberWeb.Entity.Entities;
using BerberWeb.UI.Models;
using Microsoft.AspNetCore.Identity;

namespace BerberWeb.UI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> LoginAsync(UserLoginViewModel userLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, false, false);
            if (!result.Succeeded)
            {
                return null;
            }

            else
            {
                var IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (IsAdmin) { return "Admin"; }
                var IsTeacher = await _userManager.IsInRoleAsync(user, "Personel");
                if (IsTeacher) { return "Personel"; }
                var IsStudent = await _userManager.IsInRoleAsync(user, "Customer");
                if (IsStudent) { return "Customer"; }

            }

            return null;

        }
    }
}
