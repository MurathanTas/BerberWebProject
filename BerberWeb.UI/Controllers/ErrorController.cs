using Microsoft.AspNetCore.Mvc;

namespace BerberWeb.UI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
