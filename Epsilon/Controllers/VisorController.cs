using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Controllers
{
    public class VisorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
