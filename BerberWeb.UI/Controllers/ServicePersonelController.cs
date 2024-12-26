using BerberWeb.DataAccess.Context;
using BerberWeb.Entity.Entities;
using BerberWeb.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BerberWeb.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServicePersonelController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly BerberWebDbContext _dbContext;

        public ServicePersonelController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, BerberWebDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int serviceId)
        {

            var service = await _dbContext.Services
                                          .Include(s => s.ServicePersonels)
                                          .ThenInclude(sp => sp.AppUser)  // Hizmetin personel ilişkisini yükle
                                          .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                TempData["Error"] = "Hizmet bulunamadı!";
                return RedirectToAction("Index");
            }

            var assignedPersonels = service.ServicePersonels.Select(sp => sp.AppUser).ToList();

            var model = new ServicePersonelViewModel
            {
                ServiceName = service.ServiceName,
                AssignedPersonels = assignedPersonels,
                serviceID = serviceId

            };

            return View(model);
        }


        public async Task<IActionResult> AssignPersonel(int serviceId)
        {

            var assignedPersonels = await _dbContext.ServicePersonels
                .Where(sp => sp.ServiceId == serviceId)
                .Select(sp => sp.AppUserId)
                .ToListAsync();

            var personelUsers = await _userManager.GetUsersInRoleAsync("Personel");

            var availablePersonels = personelUsers
                .Where(u => !assignedPersonels.Contains(u.Id))
                .Select(u => new SelectListItem
                {
                    Text = $"{u.FirstName} {u.LastName}",
                    Value = u.Id.ToString()
                }).ToList();

            if (availablePersonels.Count == 0)
            {
                return RedirectToAction("Index", "AdminService");
            }

            var model = new AssignPersonelViewModel
            {
                ServiceId = serviceId,
                Personels = availablePersonels
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignPersonel(AssignPersonelViewModel model)
        {


            var selectedPersonel = await _userManager.FindByIdAsync(model.SelectedPersonelId.ToString());
            if (selectedPersonel == null)
            {
                ModelState.AddModelError("", "Seçilen personel bulunamadı.");
                return View(model);
            }

            var service = await _dbContext.Services.FindAsync(model.ServiceId);
            if (service == null)
            {
                ModelState.AddModelError("", "Hizmet bulunamadı.");
                return View(model);
            }

            var servicePersonel = new ServicePersonel
            {
                ServiceId = service.Id,
                AppUserId = selectedPersonel.Id
            };
            _dbContext.ServicePersonels.Add(servicePersonel);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = "Personel başarıyla atandı.";
            return RedirectToAction("Index", "ServicePersonel", new { serviceId = model.ServiceId });
        }

        [HttpPost]
        public async Task<IActionResult> RemovePersonel(int serviceId, int personelId)
        {
            var servicePersonel = await _dbContext.ServicePersonels
                                                  .FirstOrDefaultAsync(sp => sp.ServiceId == serviceId && sp.AppUserId == personelId);

            if (servicePersonel == null)
            {
                TempData["Error"] = "Bu hizmete atanmış böyle bir personel bulunamadı.";
                return RedirectToAction("Index", new { serviceId });
            }

            _dbContext.ServicePersonels.Remove(servicePersonel);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = "Personel başarıyla silindi.";
            return RedirectToAction("Index", new { serviceId });
        }

    }
}
