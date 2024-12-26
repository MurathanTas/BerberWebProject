using BerberWeb.Business.Abstract;
using BerberWeb.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BerberWeb.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IContactService _contactService;
        private readonly IServiceService _serviceService;

        public HomeController(IAboutService aboutService, IContactService contactService, IServiceService serviceService)
        {
            _aboutService = aboutService;
            _contactService = contactService;
            _serviceService = serviceService;
        }

        public IActionResult Index()
        {

            var about = _aboutService.TGetByID(1);
            var services = _serviceService.TGetList();
            var contact = _contactService.TGetByID(1);

            var viewModel = new MainPageViewModel
            {
                About = about,
                Services = services,
                Contact = contact
            };

            return View(viewModel);
        }
    }
}
