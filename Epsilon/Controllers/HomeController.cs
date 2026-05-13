using System.Diagnostics;
using Epsilon.Models;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Metodo para abrir el documento Word del roadmap.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AbrirWord()
        {
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos", "RoadMap.docx");

            if (!System.IO.File.Exists(ruta))
                return NotFound("Archivo no encontrado");

            try
            {
                Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                return BadRequest("No se pudo abrir el archivo: " + ex.Message);
            }

            return Ok(); // No devolvemos el archivo
        }
    }
}
