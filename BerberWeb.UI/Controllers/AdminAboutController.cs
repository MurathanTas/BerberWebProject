using BerberWeb.Business.Abstract;
using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BerberWeb.UI.Controllers
{
    public class AdminAboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AdminAboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public IActionResult Index()
        {
            var about = _aboutService.TGetByID(1);
            if (about == null)
            {
                // Yeni bir About nesnesi oluştur ve varsayılan değerlerle kaydet
                about = new About
                {
                     AboutID = 1,
                    Title = "Varsayılan Başlık",
                    Description = "Varsayılan Açıklama",
                    ImageUrl = "boş"
                };

                // Yeni nesneyi kaydet
                _aboutService.TAdd(about);
            }

            return View(about);
        }

        [HttpPost]
        public IActionResult Update(About about)
        {
            if (ModelState.IsValid)
            {
                _aboutService.TUpdate(about);
                return RedirectToAction("Index");
            }

            return View(about);
        }
    }
}
