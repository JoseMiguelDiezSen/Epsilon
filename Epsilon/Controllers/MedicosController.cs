using Calipso.Security;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class MedicosController : AbstractSecurityController
    {
        private IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public MedicosController(ILogger<MedicosController> logger, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger)
        {
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
