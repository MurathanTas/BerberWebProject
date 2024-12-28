using BerberWeb.Business.Abstract;
using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BerberWeb.UI.Controllers
{
    public class AdminContactController : Controller
    {
        private readonly IGenericService<Contact> _contactService;

        public AdminContactController(IGenericService<Contact> contactService)
        {
            _contactService = contactService;
        }

        public IActionResult Index()
        {
            var contact = _contactService.TGetByID(1);
            if (contact == null)
            {
                // Yeni bir About nesnesi oluştur ve varsayılan değerlerle kaydet
                contact = new Contact
                {
                    Id = 1,
                    Description = "Varsayılan Başlık",
                    ImageUrl = "Varsayılan Açıklama",
                    Email = "boş",
                    PhoneNumber = "boş"
                };

                // Yeni nesneyi kaydet
                _contactService.TAdd(contact);
            }

            return View(contact);

        }

        [HttpPost]
        public IActionResult Update(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _contactService.TUpdate(contact);
                return RedirectToAction("Index");
            }

            return View(contact);
        }
    }
}
