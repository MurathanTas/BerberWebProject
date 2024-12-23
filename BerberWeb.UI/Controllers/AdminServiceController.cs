using BerberWeb.Business.Abstract;
using BerberWeb.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BerberWeb.UI.Controllers
{
    public class AdminServiceController : Controller
    {
        private readonly IGenericService<Service> _serviceService;

        public AdminServiceController(IGenericService<Service> serviceService)
        {
            _serviceService = serviceService;
        }

        public IActionResult Index()
        {
            var serviceList = _serviceService.TGetList()
                                             .OrderBy(x => x.Id)
                                             .ToList();
            return View(serviceList);
        }

        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddService(Service service)
        {
            _serviceService.TAdd(service);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteService(int id)
        {
            var values = _serviceService.TGetByID(id);
            _serviceService.TDelete(values.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditService(int id)
        {
            var values = _serviceService.TGetByID(id);
            return View(values);
        }

        [HttpPost]
        public IActionResult EditService(Service service)
        {
            _serviceService.TUpdate(service);
            return RedirectToAction("Index");
        }
    }
}
