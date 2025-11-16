using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;
using Negocio.Servicios.Comun;
using Microsoft.EntityFrameworkCore;
using Calipso.Security;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Epsilon.Controllers
{
    public class ConfiguracionController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IGestionUsuarios _gestionUsuarios;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public ConfiguracionController(ILogger<ConfiguracionController> logger, ISeguridad seguridad, IGestionUsuarios gestionUsuarios, IRazorRenderService renderService) : base(logger, seguridad)
        {
            _gestionUsuarios = gestionUsuarios;
            _renderService = renderService;
        }

        /// <summary> Abre ventana modal para la configuracion del correo </summary>
        [HttpGet, AjaxOnly]
        public async Task<JsonResult> ModalConfigurarCorreo()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            EpsilonDbContext context = _gestionUsuarios.Context;
            ViewFormCorreoElectronico vmConfiguracionCorreo = new ViewFormCorreoElectronico();
            vmConfiguracionCorreo.ModelosCorreo = new SelectList(context.CorreoElectronico.ToList(), nameof(CorreosElectronicos.IdCorreo), nameof(CorreosElectronicos.NombreCorreo));
            string data = await _renderService.ToStringAsync("FormConfigurarCorreo", vmConfiguracionCorreo);
            jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
            return new JsonResult(jsonResponse);
        }

        /// <summary>
        /// Obtiene los datos del modelo seleccionado
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        [HttpGet, AjaxOnly]
        public async Task<JsonResult> GetCorreoInfo(int idCorreo)
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");

            var context = _gestionUsuarios.Context;
                     var correo = context.CorreoElectronico
                         .Where(c => c.IdCorreo == idCorreo)
                         .Select(c => new { c.IdCorreo, c.Asunto, c.CuerpoMensaje, c.NombreCorreo })
                         .FirstOrDefault();

            jsonResponse.Data = JsonSerializer.Serialize(correo);
            return new JsonResult(jsonResponse); 
        }

        /// <summary> Guarda los datos introducidos en configuracion modelo correo </summary>
        [HttpPost, AjaxOnly]
        public async Task<JsonResult> AceptarConfiguracionCorreo(CorreoElectronicoViewModel vmModeloCorreo)
        {

            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            EpsilonDbContext context = _gestionUsuarios.Context;
            CorreosElectronicos correoElectronico = new CorreosElectronicos();

            if (vmModeloCorreo.IdCorreo == 0)
            {
                correoElectronico.NombreCorreo = vmModeloCorreo.NombreCorreoNuevo;
                correoElectronico.Asunto = vmModeloCorreo.Asunto;
                correoElectronico.CuerpoMensaje = vmModeloCorreo.CuerpoMensaje;
                //_gestionUsuarios.GuardarCorreoNuevo(correoElectronico);
            }
            // Actualizacion
            else
            {
                correoElectronico.IdCorreo = vmModeloCorreo.IdCorreo;
                var modeloSeleccionado = context.CorreoElectronico
                    .Where(c => c.IdCorreo == vmModeloCorreo.IdCorreo)
                    .Select(c => c.NombreCorreo)
                    .FirstOrDefault();
                correoElectronico.NombreCorreo = modeloSeleccionado;
                correoElectronico.Asunto = vmModeloCorreo.Asunto;
                correoElectronico.CuerpoMensaje = vmModeloCorreo.CuerpoMensaje;
                //_gestionUsuarios.ActualizarDatosCorreo(correoElectronico);
            }

            jsonResponse.Data = JsonSerializer.Serialize(jsonResponse);
            return new JsonResult(jsonResponse);

        }

        /// <summary>
        /// Elimina un Modelo de correo
        /// </summary>
        /// <param name="idCorreo"></param>
        /// <returns></returns>
        //[HttpPost, AjaxOnly]
        //public async Task<JsonResult> EliminarModeloCorreo(int idCorreo)
        //{
        //    JsonResponse response = new JsonResponse("200", "Ok");

        //    _formacion.EliminarModeloCorreo(idCorreo);

        //    response.StatusMessage = "Se ha eliminado el modelo de correo";
        //    response.Data = "Usuario eliminado correctamente";
        //    return new JsonResult(response);
        //}





        public IActionResult Index()
        {
            return View();
        }

    }
}
