using BerberWeb.Business.Abstract;
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
    public class RandevuController : Controller
    {
        private readonly BerberWebDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IGenericService<Service> _serviceService;

        public RandevuController(BerberWebDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IGenericService<Service> serviceService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _serviceService = serviceService;
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Index()
        {

            var services = _serviceService.TGetList()
                                            .OrderBy(x => x.Id)
                                            .ToList();

            return View(services);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(int serviceId)
        {

            var service = _serviceService.TGetByID(serviceId);
            if (service == null)
            {
                return NotFound();
            }

            var hizmetPersonel = await _context.ServicePersonels
                .Where(sp => sp.ServiceId == serviceId)
                .Include(sp => sp.AppUser)
                .Select(sp => sp.AppUser)
                .ToListAsync();

            var personelDetaylari = await _context.Personels
                .Where(p => hizmetPersonel.Select(u => u.Id).Contains(p.AppUserId))
                .ToListAsync();


            var viewModel = new RandevuViewModel
            {
                ServiceId = serviceId,
                Services = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = service.ServiceName,
                Value = serviceId.ToString()
            }
        },
                Personels = personelDetaylari.Select(p => new SelectListItem
                {
                    Text = $"{p.AppUser.FirstName} {p.AppUser.LastName}",
                    Value = p.PersonelId.ToString()
                }).ToList()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Create(RandevuViewModel model)
        {

            if (model == null || model.ServiceId == 0 || model.PersonelId == 0 || model.StartDate == default)
            {
                ModelState.AddModelError(string.Empty, "Lütfen tüm alanları doldurduğunuzdan emin olun.");
                await ModeliYukle(model);
                return View(model);
            }

            if (model.StartDate < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Randevu tarihi bugünden önce olamaz.");
                await ModeliYukle(model);
                return View(model);
            }

            var userId = (await _userManager.GetUserAsync(User)).Id;

            // Seçilen hizmeti al
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == model.ServiceId);
            if (service == null)
            {
                ModelState.AddModelError(string.Empty, "Seçilen hizmet bulunamadı.");
                await ModeliYukle(model);
                return View(model);
            }

            var serviceDuration = service.Duration ?? TimeSpan.Zero;
            if (serviceDuration == TimeSpan.Zero)
            {
                ModelState.AddModelError(string.Empty, "Hizmet süresi geçersiz.");
                await ModeliYukle(model);
                return View(model);
            }

            var personel = await _context.Personels
                .FirstOrDefaultAsync(p => p.PersonelId == model.PersonelId);

            if (personel == null)
            {
                ModelState.AddModelError(string.Empty, "Seçilen personel bulunamadı.");
                await ModeliYukle(model);
                return View(model);
            }

            var appUserId = personel.AppUserId;

            var appointmentStart = model.StartDate.ToUniversalTime();
            var appointmentFinish = appointmentStart.Add(serviceDuration);

            var isAvailable = await _context.PersonelMusaitliks
                .AnyAsync(pm =>
                    pm.AppUserId == appUserId &&
                    pm.BaslangicSaati <= appointmentStart &&
                    pm.BitisSaati >= appointmentFinish);

            if (!isAvailable)
            {
                ModelState.AddModelError(string.Empty, "Seçilen personel bu zaman aralığında müsait değil.");
                await ModeliYukle(model);
                return View(model);
            }

            //Çakışma kontrolü
            var hasConflict = await _context.Randevus
                .AnyAsync(r =>
            r.PersonelId == model.PersonelId &&
            r.StartDate < appointmentFinish &&
            r.FinishDate > appointmentStart &&
          (r.Onay == true || (r.Onay == false && r.Ret == false)));

            if (hasConflict)
            {
                ModelState.AddModelError(string.Empty, "Seçilen zaman diliminde başka bir randevu var.");
                await ModeliYukle(model);
                return View(model);
            }

            var existingAppointment = await _context.Randevus
                .AnyAsync(r => r.AppUserId == userId && r.StartDate < model.StartDate.Add(serviceDuration).ToUniversalTime() && r.FinishDate > model.StartDate.ToUniversalTime());

            if (existingAppointment)
            {
                ModelState.AddModelError(string.Empty, "Belirttiğiniz zaman diliminde zaten bir randevunuz var.");
                await ModeliYukle(model);
                return View(model);
            }


            var appointment = new Randevu
            {
                ServiceId = model.ServiceId,
                StartDate = appointmentStart,
                FinishDate = appointmentFinish,
                Onay = false,
                Ret = false,
                AppUserId = userId,
                PersonelId = model.PersonelId,
                Detay = "Sonradan detay girilecek"
            };

            _context.Randevus.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task ModeliYukle(RandevuViewModel model)
        {
            var services = await _context.Services
                .Select(s => new SelectListItem
                {
                    Text = s.ServiceName,
                    Value = s.Id.ToString()
                })
                .ToListAsync();

            var hizmetPersonel = await _context.ServicePersonels
                .Where(sp => sp.ServiceId == model.ServiceId)
                .Include(sp => sp.AppUser)
                .Select(sp => sp.AppUser)
                .ToListAsync();

            var personelDetails = await _context.Personels
                .Where(p => hizmetPersonel.Select(u => u.Id).Contains(p.AppUserId))
                .ToListAsync();

            model.Services = services;
            model.Personels = personelDetails.Select(p => new SelectListItem
            {
                Text = $"{p.AppUser.FirstName} {p.AppUser.LastName}",
                Value = p.PersonelId.ToString()
            }).ToList();
        }

        private void RejectExpiredAppointments()
        {
            var suresiGecenRandevular = _context.Randevus
                .Where(r => !r.Onay && !r.Ret && r.StartDate < DateTimeOffset.UtcNow)
                .ToList();

            foreach (var appointment in suresiGecenRandevular)
            {
                appointment.Ret = true;
            }

            _context.SaveChanges();
        }




        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ApproveAppointment(int randevuId)
        {
            var randevu = _context.Randevus.FirstOrDefault(r => r.RandevuId == randevuId);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Onay = true;
            _context.SaveChanges();

            return RedirectToAction("Randevular");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RejectAppointment(int randevuId)
        {
            var randevu = _context.Randevus.FirstOrDefault(r => r.RandevuId == randevuId);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Ret = true;
            _context.SaveChanges();

            return RedirectToAction("Randevular");
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyAppointments()
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;
            var appointments = _context.Randevus
                .Where(r => r.AppUserId == userId)
                .Include(r => r.Service)
                .Include(r => r.Personel)
                .ThenInclude(r => r.AppUser)
                .ToList();

            return View(appointments);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Randevular()
        {
            RejectExpiredAppointments();

            var randevular = await _context.Randevus
        .Include(r => r.Service)
        .Include(r => r.AppUser)
        .Include(r => r.Personel)
        .ThenInclude(p => p.AppUser)
        .OrderByDescending(r => r.Onay)
        .ThenBy(r => r.Ret)
        .ThenBy(r => r.StartDate)
        .ToListAsync();

            return View(randevular);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> DeleteRandevu(int id)
        {
            var randevu = _context.Randevus.Find(id);
            _context.Randevus.Remove(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyAppointments");
        }






    }
}
