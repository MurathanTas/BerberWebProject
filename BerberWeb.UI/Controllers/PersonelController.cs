using BerberWeb.DataAccess.Context;
using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BerberWeb.UI.Controllers
{
    [Authorize(Roles = "Personel")]
    public class PersonelController : Controller
    {
        private readonly BerberWebDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PersonelController(BerberWebDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index2()
        {
            var kullanici = await _userManager.GetUserAsync(User);
            var appUserId = kullanici.Id;

            var personel = await _context.Personels.FirstOrDefaultAsync(p => p.AppUserId == appUserId);
            if (personel == null)
            {
                return NotFound("Personel bulunamadı.");
            }

            var personelId = personel.PersonelId;

            // Personelin sadece kabul edilen randevularını al
            var appointments = await _context.Randevus
                .Where(r => r.PersonelId == personelId && r.Onay)
                .Include(r => r.Service)
                .Include(r => r.AppUser)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> CalismaSaatleri()
        {
            var personel = await _userManager.GetUserAsync(User);
            var appUserId = personel.Id;

            var calismaSaatleri = await _context.PersonelMusaitliks
                .Where(p => p.AppUserId == appUserId)
                .ToListAsync();

            return View(calismaSaatleri);
        }
    }
}
