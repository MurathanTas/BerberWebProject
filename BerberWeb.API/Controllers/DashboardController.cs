using BerberWeb.DataAccess.Context;
using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BerberWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] 
    public class DashboardController : Controller
    {
        private readonly BerberWebDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(BerberWebDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // Günlük en çok randevusu olan personel
        [HttpGet("top-employee-today")]
        //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopEmployeeToday()
        {
            var today = DateTimeOffset.Now.Date;

            var topEmployee = await _context.Randevus
                .Where(a => a.StartDate.Date == today)
                .GroupBy(a => a.PersonelId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    PersonelId = g.Key,
                    AppointmentCount = g.Count(),
                    FullName = g.FirstOrDefault().Personel.AppUser.FirstName + " " + g.FirstOrDefault().Personel.AppUser.LastName
                })
                .FirstOrDefaultAsync();

            return Ok(topEmployee);
        }

        [HttpGet("top-employee-this-monthly")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopEmployeeMonthly()
        {
            var startOfMonth = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, TimeSpan.Zero);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var topEmployeeMonthly = await _context.Randevus
                .Where(r => r.StartDate >= startOfMonth && r.StartDate <= endOfMonth && r.Onay == true) 
                .GroupBy(r => r.PersonelId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    PersonelId = g.Key,
                    AppointmentCount = g.Count(),
                    Kazanc = g.Sum(r => (decimal?)r.Service.Price ?? 0), 
                    FullName = g.FirstOrDefault().Personel.AppUser.FirstName + " " + g.FirstOrDefault().Personel.AppUser.LastName
                })
                .FirstOrDefaultAsync();

            return Ok(topEmployeeMonthly);
        }

        // Şu ana kadar en çok gelen müşteri
        [HttpGet("top-customer")]
        //  [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopCustomer()
        {
            var topCustomer = await _context.Randevus
                .GroupBy(ar => ar.AppUserId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    CustomerId = g.Key,
                    AppointmentCount = g.Count(),
                    FullName = g.FirstOrDefault().AppUser.FirstName + " " + g.FirstOrDefault().AppUser.LastName
                })
                .FirstOrDefaultAsync();

            return Ok(topCustomer);
        }

        [HttpGet("top-service")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopService()
        {
            var topService = await _context.Randevus
                .GroupBy(a => a.ServiceId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    ServiceId = g.Key,
                    AppointmentCount = g.Count(),
                    ServiceName = g.FirstOrDefault().Service.ServiceName
                })
                .FirstOrDefaultAsync();

            return Ok(topService);
        }

        // Günlük Kazanç (Admin için)
        [HttpGet("daily-revenue")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDailyRevenue()
        {
            var today = DateTimeOffset.Now.Date;

            var dailyRevenue = await _context.Randevus
                .Where(a => a.StartDate.Date == today && a.Onay == true)
                .SumAsync(a => (decimal?)a.Service.Price ?? 0); // Eğer hizmet fiyatı null ise 0 olarak ele alınır.

            return Ok(new
            {
                Date = today,
                Revenue = dailyRevenue
            });
        }

        // Aylık Kazanç (Admin için)
        [HttpGet("monthly-revenue")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMonthlyRevenue()
        {
            var startOfMonth = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, TimeSpan.Zero);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var monthlyRevenue = await _context.Randevus
                .Where(a => a.StartDate >= startOfMonth && a.StartDate <= endOfMonth && a.Onay == true)
                .SumAsync(a => (decimal?)a.Service.Price ?? 0);

            return Ok(new
            {
                Month = startOfMonth.ToString("MMMM yyyy"), // Örnek: "December 2024"
                Revenue = monthlyRevenue
            });
        }

    }
}
