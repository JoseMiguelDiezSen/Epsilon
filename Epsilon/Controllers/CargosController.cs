using Calipso.Security;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class CargosController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionPacientes _gestionPacientes;

        /// <summary>
        /// Constructor del controlador 'Pacientes'
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionPacientes"></param>
        public CargosController(ILogger<CargosController> logger, ISeguridad seguridad, IGestionPacientes gestionPacientes, IRazorRenderService renderService) : base(logger, seguridad)
        {
            _gestionPacientes = gestionPacientes;
            _renderService = renderService;
        }



        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
