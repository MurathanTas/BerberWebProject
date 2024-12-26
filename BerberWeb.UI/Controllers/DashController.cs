using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BerberWeb.UI.Controllers
{

    [Authorize(Roles = "Admin")]
    public class DashController : Controller
    {
        private readonly HttpClient _httpClient;

        public DashController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7283/api/"); // API Base URL

        }

        public async Task<IActionResult> Index()
        {
            // Günlük kazanç
            var dailyRevenueResponse = await _httpClient.GetAsync("dashboard/daily-revenue");
            if (dailyRevenueResponse.IsSuccessStatusCode)
            {
                var json = await dailyRevenueResponse.Content.ReadAsStringAsync();
                var dailyRevenue = JsonConvert.DeserializeObject<DailyRevenueViewModel>(json);
                ViewBag.DailyRevenue = dailyRevenue;
            }
            else
            {
                ViewBag.DailyRevenueError = "Günlük kazanç verisi alınamadı.";
            }

            // Aylık kazanç
            var monthlyRevenueResponse = await _httpClient.GetAsync("dashboard/monthly-revenue");
            if (monthlyRevenueResponse.IsSuccessStatusCode)
            {
                var json = await monthlyRevenueResponse.Content.ReadAsStringAsync();
                var monthlyRevenue = JsonConvert.DeserializeObject<MonthlyRevenueViewModel>(json);
                ViewBag.MonthlyRevenue = monthlyRevenue;
            }
            else
            {
                ViewBag.MonthlyRevenueError = "Aylık kazanç verisi alınamadı.";
            }

            // Günlük en çok randevusu olan personel
            var topEmployeeResponse = await _httpClient.GetAsync("dashboard/top-employee-today");
            if (topEmployeeResponse.IsSuccessStatusCode)
            {
                var json = await topEmployeeResponse.Content.ReadAsStringAsync();
                var topEmployee = JsonConvert.DeserializeObject<TopEmployeeTodayViewModel>(json);
                ViewBag.TopEmployeeToday = topEmployee;
            }
            else
            {
                ViewBag.TopEmployeeTodayError = "En çok randevusu olan personel verisi alınamadı.";
            }
            // Aylık en çok işi olan personel.
            var topEmployeeThisMonthResponse = await _httpClient.GetAsync("dashboard/top-employee-this-monthly");
            if (topEmployeeThisMonthResponse.IsSuccessStatusCode)
            {
                var json = await topEmployeeThisMonthResponse.Content.ReadAsStringAsync();
                var topEmployeeThisMonth = JsonConvert.DeserializeObject<TopEmployeeMonthlyViewModel>(json);
                ViewBag.TopEmployeeThisMonth = topEmployeeThisMonth;
            }
            else
            {
                ViewBag.TopEmployeeThisMonthError = "Aylık en çok randevusu olan personel verisi alınamadı.";
            }
            // En çok gelen müşteri
            var topCustomerResponse = await _httpClient.GetAsync("dashboard/top-customer");
            if (topCustomerResponse.IsSuccessStatusCode)
            {
                var json = await topCustomerResponse.Content.ReadAsStringAsync();
                var topCustomer = JsonConvert.DeserializeObject<TopCustomerViewModel>(json);
                ViewBag.TopCustomer = topCustomer;
            }
            else
            {
                ViewBag.TopCustomerError = "En çok gelen müşteri verisi alınamadı.";
            }

            // En çok randevusu alınan hizmet
            var topServiceResponse = await _httpClient.GetAsync("dashboard/top-service");
            if (topServiceResponse.IsSuccessStatusCode)
            {
                var json = await topServiceResponse.Content.ReadAsStringAsync();
                var topService = JsonConvert.DeserializeObject<TopServiceViewModel>(json);
                ViewBag.TopService = topService;
            }
            else
            {
                ViewBag.TopServiceError = "En çok randevusu alınan hizmet verisi alınamadı.";
            }

            return View();
        }
    }

    public class DailyRevenueViewModel
    {
        public string Date { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MonthlyRevenueViewModel
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class TopEmployeeTodayViewModel
    {
        public int PersonelId { get; set; }
        public int AppointmentCount { get; set; }
        public string FullName { get; set; }
    }

    public class TopEmployeeMonthlyViewModel
    {
        public int PersonelId { get; set; }
        public int AppointmentCount { get; set; }
        public int Kazanc { get; set; }
        public string FullName { get; set; }
    }

    public class TopCustomerViewModel
    {
        public int CustomerId { get; set; }
        public int AppointmentCount { get; set; }
        public string FullName { get; set; }
    }

    public class TopServiceViewModel
    {
        public int ServiceId { get; set; }
        public int AppointmentCount { get; set; }
        public string ServiceName { get; set; }
    }
}
