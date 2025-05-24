using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;


namespace Epsilon.Controllers
{
    public class TratamientosController : AbstractSecurityController
    {
        private readonly IRazorRenderService _razorRenderService;
        private IGestionUsuarios _gestionUsuarios;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public TratamientosController(ILogger<TratamientosController> logger, ISeguridad seguridad, IGestionUsuarios gestionUsuarios,IRazorRenderService renderService) : base(logger, seguridad)
        {
            _gestionUsuarios = gestionUsuarios;
            _razorRenderService = renderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AgregarTratamiento

        /// <summary>
        /// Método para abrir la ventana modal de agregar tratamiento
        /// </summary>
        /// <returns></returns>

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarUsuario()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarTratamiento vmAgregarTratamiento = new ViewFormAgregarTratamiento();
            vmAgregarTratamiento.IdTratamiento = 1;
            //vmAgregarTratamiento.NombreTratamiento = new SelectList(nameOF(Tratamiento.IdTratamiento), Tratamiento.NombreTratamiento);
            string data = await _razorRenderService.ToStringAsync("FormAddTratamiento", vmAgregarTratamiento);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }



        /// <summary>
        /// Método que contiene la funcionalidad de Añadir Periodos
        /// </summary>
        /// <param name="vmperiodo"></param>
        /// <returns></returns>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> AgregarUsuarioAsync(ViewFormAgregarUsuario vmUsuario)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                Usuario usuario = new Usuario()
                {
                    Nombre = vmUsuario.Nombre,
                    Password = vmUsuario.Password,
                    Email = vmUsuario.Email,
                    FechaAlta = vmUsuario.FechaAlta,
                    Telefono = vmUsuario.Telefono,
                    RutaFoto = vmUsuario.RutaFoto,
                    Activo = vmUsuario.Activo,

                };

                _gestionUsuarios.AddUser(usuario);
                result = new JsonResult(new { StatusCode = 200, message = "Usuario agregado correctamente" });
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return result;
        }

        #endregion








    }
}
