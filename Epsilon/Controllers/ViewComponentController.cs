using Epsilon.Attributes;
using Epsilon.Renders;
using Microsoft.AspNetCore.Mvc;
using Negocio.Servicios.Comun;
using Negocio.Servicios;
using Calipso.Security;
using Epsilon.Models.Comun;

namespace Epsilon.Controllers
{
    public class ViewComponentController : AbstractSecurityController
    {
        private readonly IRazorRenderService _renderService;
        private IPlanificacion _planificacion;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="logger"> Servicio de logs de la aplicación </param>
        /// <param name="planificacion"> Servicio con las funciones de planificacion de la aplicación </param>
        /// <param name="seguridad"> Servicio con las funciones de seguridad de la aplicación </param>
        /// <param name="renderService"> Motor personalizado para la rederización a HTML de vistas parciales </param>
        public ViewComponentController(ILogger<ViewComponentController> logger, ISeguridad seguridad, IRazorRenderService renderService, IPlanificacion planificacion) : base(logger, seguridad)
        {
            _renderService = renderService;
            _planificacion = planificacion;
        }

        /// <summary>
        /// Metodo de acceso a la pagina 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }

        /// <summary> Método encargado de guardar las preferencias de Cursos </summary>
        //[HttpPost, AjaxOnly]
        //public async Task<JsonResult> SetPreferenciasCurso(long idCurso)
        //{

        //    //PreferenciasUsuarios? preferenciaUsuario = _seguridad.GetPreferenciasUsuario(_seguridad.CurrentProgesforUser?.IDP ?? 0, "Curso");
        //    //if (!idCurso.ToString().Equals(preferenciaUsuario?.Valor))
        //    //{
        //    //    PreferenciasUsuarios preferenciasDB = new PreferenciasUsuarios();
        //    //    ViewSelectorCursosEdiciones vmSelectorCurso = new ViewSelectorCursosEdiciones();
        //    //    DatosCursoPlanificado? cursohistorial = new DatosCursoPlanificado();

        //    //    cursohistorial = _seguridad.Context.DatoCursoPeriodo.Where(x => x.IdCursoPlanificado == idCurso).ToList().FirstOrDefault();
        //    //    vmSelectorCurso.IdCurso = cursohistorial?.IdCursoPlanificado ?? 0;
        //    //    vmSelectorCurso.NombreCurso = cursohistorial?.Curso ?? "";
        //    //    vmSelectorCurso.NumeroCurso = cursohistorial?.Contador ?? 0;
        //    //    vmSelectorCurso.NombrePlan = cursohistorial?.NombrePlan ?? "";
        //    //    vmSelectorCurso.NombreSubPlan = cursohistorial?.NombreSubPlan ?? "";
        //    //    vmSelectorCurso.Ejercicio = cursohistorial?.Ejercicio ?? 0;

        //    //    int idp = _seguridad?.CurrentProgesforUser?.IDP ?? 0;
        //    //    _seguridad?.Context.EjecutarEnTransaccion((trans) =>
        //    //    {
        //    //        if (preferenciaUsuario != null)
        //    //        {
        //    //            preferenciaUsuario.Valor = idCurso.ToString();
        //    //            _seguridad.UpadatePreferenciaUsuario(preferenciaUsuario);
        //    //        }
        //    //        else
        //    //        {
        //    //            preferenciasDB.Nombre = "Curso";
        //    //            preferenciasDB.Valor = idCurso.ToString();
        //    //            preferenciasDB.Tipo = "long";
        //    //            preferenciasDB.IDP = _seguridad.CurrentProgesforUser?.IDP ?? 0;
        //    //            _seguridad.AddPreferenciaUsuario(preferenciasDB);
        //    //        }
        //    //        _seguridad.DeletePreferenciasUsuarios(idp, "Edicion");
        //    //        return true;
        //    //    });
        //    //}
        //    return response;

        //}

        #region Selector_Ediciones

        /// <summary> Método encargado de guardar las preferencias de Ediciones </summary>
    //    [HttpPost, AjaxOnly]
    //    public async Task<JsonResult> SetPreferenciasEdicion(long Edicion)
    //    {

    //        //PreferenciasUsuarios prefernciabd = new PreferenciasUsuarios();
    //        ////Busco la preferencia  
    //        //PreferenciasUsuarios? preferencia = _seguridad.GetPreferenciasUsuario(_seguridad.CurrentProgesforUser?.IDP ?? 0, "Edicion");

    //        //if (preferencia != null)
    //        //{
    //        //    preferencia.Valor = Edicion.ToString();
    //        //    _seguridad.UpadatePreferenciaUsuario(preferencia);
    //        //}
    //        //else
    //        //{
    //        //    prefernciabd.Nombre = "Edicion";
    //        //    prefernciabd.Valor = Edicion.ToString();
    //        //    prefernciabd.Tipo = "long";
    //        //    prefernciabd.IDP = _seguridad.CurrentProgesforUser?.IDP ?? 0;
    //        //    _seguridad.AddPreferenciaUsuario(prefernciabd);
    //        //}
    //    //    return response;

    //    //}

    //    ///// <summary> Método encargado de obtener las ediciones de un curso </summary>
    //    //[HttpGet, AjaxOnly]
    //    //public async Task<JsonResult> GetCursosEdiciones([FromQuery] string options, long IdCurso)
    //    //{
    //    //    JsonResponse jsonResponse = ("",,"");
    //    //             //IQueryable<DatosEdicionEjecucion> cursosejecutados = _seguridad.Context.DatosEdicionEjecucion.Where(n => n.IdArea == idArea && n.IdCurso.Equals(IdCurso) && _seguridad.Context.HayPermisoConsulta(idp, n.IdEntidadSecurizada ?? 0)).OrderBy(o => o.NumeroEdicion);
    //    //             //DSLoadOptions? loadOptions = JsonSerializer.Deserialize<DSLoadOptions>(options);
    //    //             //if (loadOptions != null)
    //    //             //{
    //    //             //    if (loadOptions.Filter != null)
    //    //             //    {
    //    //             //        // Procesa filtros DataGrid
    //    //             //        List<ExpressionFilter> filtros = UtilidadesDX.PreparaFiltros<DatosEdicionEjecucion>(loadOptions.Filter);
    //    //             //        Expression<Func<DatosEdicionEjecucion, bool>>? exFiltro = UtilidadesDX.PreparaExpresionFiltros<DatosEdicionEjecucion>(filtros);
    //    //             //        if (exFiltro != null)
    //    //             //        {
    //    //             //            cursosejecutados = cursosejecutados.Where(exFiltro);
    //    //             //        }
    //    //             //    }

    //    //             //    if (loadOptions.Sort != null)
    //    //             //    {
    //    //             //        // Procesa ordenación DataGrid
    //    //             //        cursosejecutados = ExpressionBuilder.OrderBy(cursosejecutados, loadOptions.Sort[0].Selector, loadOptions.Sort[0].Desc);
    //    //             //    }

    //    //             //    if (loadOptions.Take > 0)
    //    //             //    {
    //    //             //        // Procesa paginación DataGrid
    //    //             //        cursosejecutados = cursosejecutados.Skip(loadOptions.Skip).Take(loadOptions.Take);
    //    //             //    }

    //    //             //}

    //    //             //response.Data = JsonSerializer.Serialize(await cursosejecutados.ToListAsync());
    //    //             return jsonResponse;
    //    //}

    #endregion



    }
}
