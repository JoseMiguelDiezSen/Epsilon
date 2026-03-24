using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class AgendaController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public AgendaController(ILogger<AgendaController> logger, ISeguridad seguridad, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger, seguridad)
        {
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarCita(DateTime date)
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarUsuario vmAgregarUsuario = new ViewFormAgregarUsuario();
            vmAgregarUsuario.FechaAlta = DateTime.Now;
            //vmAgregarUsuario.TurnoDeTrabajo = new SelectList(_gestionUsuarios.Context.Usuarios.ToList(), nameof(Usuario.IdUsuario), nameof(Usuario.TurnoDeTrabajo));
            string data = await _renderService.ToStringAsync("FormAddCita", vmAgregarUsuario);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }
        [JSInvokable]
        public static Task OnDateClicked(string date)
        {
            Console.WriteLine("Clic en: " + date);
            return Task.CompletedTask;
        }
    }
}
