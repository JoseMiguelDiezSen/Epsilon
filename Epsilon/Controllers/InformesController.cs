using Calipso.Security;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class InformesController : AbstractSecurityController
    {
        private IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;
        private IInformes _informes;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public InformesController(ILogger<InformesController> logger, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService, IInformes informes) : base(logger)
        {
            _informes = informes;
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerarInforme(long idUsuario)
        {
            byte[] data = _informes.GeneraInforme("/EPSILON/Usuarios", "InformeUsuario", new Dictionary<String, String> { { "idUsuario", idUsuario.ToString() } });
            return File(data, "application/pdf");
        }
    }
}
