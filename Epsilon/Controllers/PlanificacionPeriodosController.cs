using Calipso.Security;
using Epsilon.Attributes;
using Epsilon.Models;
using Epsilon.Models.Comun;
using Epsilon.Renders;
using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios;

namespace Epsilon.Controllers
{
    public class PlanificacionPeriodosController : AbstractSecurityController
    {
        private IPlanificacion _planificacion;
        private readonly IRazorRenderService _razorRenderService;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="seguridad"></param>
        /// <param name="planificacion"></param>
        /// <param name="renderService"></param>
        public PlanificacionPeriodosController(ILogger<PlanificacionPeriodosController> logger, ISeguridad seguridad, IPlanificacion planificacion, IRazorRenderService renderService) : base(logger, seguridad)
        {
            _planificacion = planificacion;
            _razorRenderService = renderService;
        }

        /// <summary>
        /// Método principal de entrada a la pantalla de Planificacion de Periodos 
        /// </summary>
        /// <returns>La vista principal de la pantalla de Planificacion de Periodos</returns>
        //public IActionResult Index()
        //{
        //    PlanificacionPeriodosViewModel vmPPeriodos = new PlanificacionPeriodosViewModel();
        //    //vmPPeriodos.Periodos = new List<ViewPeriodos>() { };
        //    IQueryable<DatoPeriodo> datosPeriodos = _planificacion.GetDatosPeriodos();
        //    IEnumerable<ViewPeriodos> periodos = new List<ViewPeriodos>();

        //    datosPeriodos = datosPeriodos.Skip((vmPPeriodos.PaginaActual - 1) * vmPPeriodos.RegistrosPorPagina).Take(vmPPeriodos.RegistrosPorPagina);

        //    if (datosPeriodos.Any())
        //    {
        //        periodos = datosPeriodos.Select(x => new ViewPeriodos(x)).ToList();
        //    }

        //    vmPPeriodos.Periodos = periodos;
        //    return View("Index", vmPPeriodos);
        //}

        /// <summary> Metodo utilizado para el filtrado de periodos en funcion de los criterios seleccionados</summary>
        /// <param name="vmPeriodosPlanificacion"></param>
        /// <returns> Devuelve una lista de los periodos que coincidan con los datos introducidos</returns>
        //[HttpPost, AjaxOnly]
        //public async Task<JsonResult> FiltraPeriodosAsync(PlanificacionPeriodosViewModel vmPeriodosPlanificacion)
        //{
        //    JsonResponse? jsonResponse = null;

        //    try
        //    {
        //        var dbContext = _seguridad.Context;
        //        IQueryable<DatoPeriodo> datosPeriodos = _planificacion.GetDatosPeriodos();

        //        if (vmPeriodosPlanificacion.Ejercicio != null)
        //        {
        //            datosPeriodos = datosPeriodos.Where(p => (p.Ejercicio == vmPeriodosPlanificacion.Ejercicio));
        //        }
        //        if (vmPeriodosPlanificacion.Estimado != null)
        //        {
        //            datosPeriodos = datosPeriodos.Where(p => (p.Estimado == vmPeriodosPlanificacion.Estimado));
        //        }
        //        if (vmPeriodosPlanificacion.Ejecutado != null)
        //        {
        //            datosPeriodos = datosPeriodos.Where(p => (p.Ejecutado == vmPeriodosPlanificacion.Ejecutado));
        //        }

        //        vmPeriodosPlanificacion.Periodos = await datosPeriodos.
        //            OrderBy(x => x.Ejercicio).
        //            Skip((vmPeriodosPlanificacion.PaginaActual - 1) * vmPeriodosPlanificacion.RegistrosPorPagina).Take(vmPeriodosPlanificacion.RegistrosPorPagina).
        //            Select(x => new ViewPeriodos(x)).
        //            ToListAsync();

        //        string data = await _razorRenderService.ToStringAsync("TablaPeriodos", vmPeriodosPlanificacion.Periodos);
        //        jsonResponse = new JsonResponse("200", "Operación realizada correctamente.", data);
        //    }

        //    catch (Exception ex)
        //    {
        //        jsonResponse = new JsonResponse("500", "La operación no se pudo realizar.", String.Empty, "Error: " + ex.Message);
        //    }

        //    return new JsonResult(jsonResponse);
        //}

        //#region AgregarPeriodo

        ///// <summary>
        ///// Método para abrir la ventana modal de agregar periodos
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, AjaxOnly]
        //public async Task<ActionResult> ModalAgregarPeriodoAsync()
        //{
        //    ViewFormAgregarPeriodo viewAgregarPeriodo = new ViewFormAgregarPeriodo();
        //    return PartialView("FormAgreagarPeriodo", viewAgregarPeriodo);
        //}

        ///// <summary>
        ///// Método que contiene la funcionalidad de Añadir Periodos
        ///// </summary>
        ///// <param name="vmperiodo"></param>
        ///// <returns></returns>
        //[HttpPost, AjaxOnly]
        //public async Task<JsonResult> AgregarPeriodoAsync(ViewFormAgregarPeriodo vmperiodo)
        //{
        //    JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

        //    try
        //    {
        //        PeriodoPlanificacion periodosPlanificacion = new PeriodoPlanificacion()
        //        {
        //            //Ejercicio = vmperiodo.Ejercicio,
        //            //Desde = vmperiodo.Desde,
        //            //PlanesAfectados = "es",
        //            //Hasta = vmperiodo.Hasta,
        //            //FechaCreacion = DateTime.Now
        //        };

        //        _planificacion.AddPeriodo(periodosPlanificacion);
        //        result = new JsonResult(new { StatusCode = 200, message = "Periodo agregado correctamente" });
        //    }

        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return result;
        //}

        //#endregion

        //#region ModificarPeriodo

        //[HttpGet, AjaxOnly]
        //public async Task<ActionResult> ModalModificarPeriodoAsync(long id)
        //{
        //    PeriodoPlanificacion periodo = new PeriodoPlanificacion();
        //    periodo = _planificacion.Context.PeriodosPlanificacion.Where(x => x.IdPeriodo == id).First();

        //    ViewFormAgregarPeriodo viewAgregarPeriodo = new ViewFormAgregarPeriodo();
        //    //viewAgregarPeriodo.Ejercicio = periodo.Ejercicio;
        //    //viewAgregarPeriodo.Desde = periodo.Desde;
        //    //viewAgregarPeriodo.Hasta = periodo.Hasta;
        //    return PartialView("FormModificarPeriodo", viewAgregarPeriodo);
        //}

        ///// <summary>
        ///// Método que contiene la funcionalidad de Modificar Periodos
        ///// </summary>
        ///// <param name="vmperiodo"></param>
        ///// <returns></returns>
        //[HttpPost, AjaxOnly]
        //public async Task<JsonResult> ModificarPeriodoAsync(ViewFormAgregarPeriodo vmperiodo)
        //{
        //    JsonResult result = new JsonResult(new { StatusCode = 500, message = "No se pudo realizar la operación solicitada" });

        //    //var periodo = _planificacion.GetPeriodoPlanificacion(vmperiodo.Ejercicio);

        //    try
        //    {
        //        PeriodoPlanificacion periodosPlanificacion = new PeriodoPlanificacion()
        //        {
        //            //IdPeriodo = vmperiodo.IdPeriodo,
        //            //Ejercicio = vmperiodo.Ejercicio,
        //            //Desde = vmperiodo.Desde,
        //            //PlanesAfectados = "es",
        //            //Hasta = vmperiodo.Hasta,
        //            //FechaCreacion = DateTime.Now
        //        };

        //        var res = _planificacion.UpdatePeriodo(periodosPlanificacion);
        //        result = new JsonResult(new { StatusCode = 200, message = "Periodo agregado correctamente" });
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    return result;
        //}

        //#endregion

        //#region EliminarPeriodo 

        ///// <summary>
        ///// Metodo para eliminar un periodo de planificacion 
        ///// </summary>
        ///// <param name="id"> Identificador del periodo a eliminar </param>
        ///// <returns></returns>
        //[HttpPost, AjaxOnly]
        //public async Task<JsonResult> EliminarPeriodoAsync(long id)
        //{

        //    JsonResult response = new JsonResult("200", "Ok");
        //    try
        //    {
        //        PeriodoPlanificacion periodo = new PeriodoPlanificacion();
        //        //periodo = _planificacion.Context.PeriodosPlanificacion.Where(x => x.IdPeriodo == id).First();
        //        //_planificacion.DeletePeriodo(periodo.IdPeriodo);
        //        //response.Data = "Periodo eliminado correctamente";
        //    }
        //    catch (Exception ex)
        //    {
        //        //response.Status = "500";
        //        //response.StatusMessage = "Se ha producido un error al intentar eliminar el Periodo";
        //        //response.ErrorData = ex.ToString();
        //        //response.Data =
        //        ex.Message.ToString();
        //    }
        //    return new JsonResult(response);
        //}

        //#endregion

    }
}
