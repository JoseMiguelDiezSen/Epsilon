using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class SedesController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private EpsilonDbContext _context;

        public SedesController(ILogger<SedesController> logger, EpsilonDbContext context, IRazorRenderService renderService) : base(logger)
        {
            _context = context;
            _renderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, AjaxOnly]
        public IActionResult GetSedes()
        {
            var sedes = _context.Sedes.ToList();
            return Json(new { data = sedes });
        }
    }
}
