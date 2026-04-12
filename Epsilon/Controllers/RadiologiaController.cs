using Calipso.Security;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class RadiologiaController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public RadiologiaController(ILogger<RadiologiaController> logger, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger)
        {
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //C:\Users\josem\source\repos\JoseMiguelDiezSen\Epsilon\Epsilon\wwwroot\media\Radiografias\1.2.410.200001.101.11.801.1142083067.3.20260204190542115.dcm


    }
}
