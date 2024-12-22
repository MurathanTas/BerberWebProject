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
            var about = _contactService.TGetByID(1);
            return View(about);
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
