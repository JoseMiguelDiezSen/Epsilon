using Calipso.Security;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class Escaner3DController : AbstractSecurityController
    {
        private readonly IRazorRenderService _razorRenderService;



        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public Escaner3DController(ILogger<Escaner3DController> logger, ISeguridad seguridad, IGestionClinica gestionclinica, IRazorRenderService renderService) : base(logger, seguridad)
        {
            //            _gestionClinica = gestionclinica;
            _razorRenderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
