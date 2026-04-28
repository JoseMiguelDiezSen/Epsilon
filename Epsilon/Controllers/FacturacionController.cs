using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class FacturacionController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public FacturacionController(ILogger<FacturacionController> logger, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger)
        {
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalCalcularFactura()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarPaciente vmAgregarPaciente = new ViewFormAgregarPaciente();
            vmAgregarPaciente.FechaAlta1 = DateTime.Now;
            string data = await _renderService.ToStringAsync("FormCalcularFactura", vmAgregarPaciente);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }
    }
}
