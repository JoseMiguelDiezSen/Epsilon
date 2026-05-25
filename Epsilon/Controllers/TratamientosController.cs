using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class TratamientosController : AbstractSecurityController
    {
        private readonly IRazorRenderService _razorRenderService;
        private IGestionClinica _gestionClinica;

        /// <summary>
        /// Constructor d
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="gestionUsuarios"></param>
        public TratamientosController(ILogger<TratamientosController> logger, IGestionClinica gestionclinica, IRazorRenderService renderService) : base(logger)
        {
            _gestionClinica = gestionclinica;
            _razorRenderService = renderService;
        }

        public IActionResult Index()
        {
            TratamientosViewModel vmTratamientos = new TratamientosViewModel();

            IQueryable<DatosTratamientos> datosTratamientos = _gestionClinica
                .GetTratamientos()
                .AsNoTracking()
                .OrderBy(x => x.IdTratamiento)
                .Skip((vmTratamientos.PaginaActual - 1) * vmTratamientos.RegistrosPorPagina)
                .Take(vmTratamientos.RegistrosPorPagina);

            var tratamientos = datosTratamientos
                .ToList() // aquí materializas
                .Select(x => new ViewTratamiento(x)) // aquí usas tu constructor
                .ToList();

            vmTratamientos.Tratamientos = tratamientos;

            return View("Index", vmTratamientos);
        }

        #region AgregarTratamiento

        /// <summary>
        /// Método para abrir la ventana modal de agregar tratamiento
        /// </summary>
        /// <returns></returns>

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ModalAgregarTratamiento()
        {
            JsonResponse? jsonResponse = new JsonResponse("400", "Error en el servidor", "");
            ViewFormAgregarTratamiento vmAgregarTratamiento = new ViewFormAgregarTratamiento();
            vmAgregarTratamiento.NombreTratamiento = new SelectList(_gestionClinica.Context.Tratamientos.ToList(), nameof(Tratamiento.IdTratamiento), nameof(Tratamiento.NombreTratamiento));
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
        public async Task<JsonResult> AgregarTratamientoAsync(ViewFormAgregarTratamiento vmTratamiento)
        {
            JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

            try
            {
                Tratamiento tratamiento = new Tratamiento()
                {
                    //Nombre = vmUsuario.Nombre,
                    //Password = vmUsuario.Password,
                    //Email = vmUsuario.Email,
                    //FechaAlta = vmUsuario.FechaAlta,
                    //Telefono = vmUsuario.Telefono,
                    //RutaFoto = vmUsuario.RutaFoto,
                    //Activo = vmUsuario.Activo,

                };

                //_gestionClinica.AddTratamiento(tratamiento);
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
