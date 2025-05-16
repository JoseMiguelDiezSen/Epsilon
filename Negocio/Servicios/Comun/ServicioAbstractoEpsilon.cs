using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using System.Diagnostics;
using System.Reflection;

namespace Negocio.Servicios.Comun
{
    public abstract class ServicioAbstractoEpsilon : ServicioAbstracto<EpsilonDbContext>
    {
        public ServicioAbstractoEpsilon(EpsilonDbContext context, ILogger milogger) : base(context, milogger)
        {

        }

        //protected void CheckPermissions(NivelesPrelacion nivelRequerido)
        //{
        //    MethodBase? caller = new StackTrace().GetFrame(1)?.GetMethod();
        //    string servicio = GetType().Name ?? string.Empty;
        //    string clase = GetType().FullName ?? string.Empty;
        //    string operacion = caller?.Name ?? string.Empty;
        //    string usuario = CurrentCalipsoforUser?.Nombre ?? String.Empty;

        //    // Comprueba si el usuario tiene nivel suficiente
        //    if (!IsAccessAllowed(nivelRequerido)){
        //        throw new UnauthorizedAccessException($"El usuario inactivo, {usuario}, ha intentado ejecutar la operacion {operacion}, del servicio {servicio}.");
        //    }

        //    if (IsAccessAllowed(nivelRequerido)) {
        //        throw new UnauthorizedAccessException($"Se ha intentado ejecutar la operacion, { operacion } del servicio {servicio} sin el nivel requerido.");
        //    }

        //    Funcionalidad funcionalidad = new Funcionalidad()
        //    { Clase = clase, Operacion = operacion };

        //    if (!IsExecutionAllowed(funcionalidad)) {
        //        throw new UnauthorizedAccessException($"Se ha intentado ejecutar la operacion, {operacion} del servicio {servicio} sin el nivel requerido.");
        //    }
        //}

        //protected bool IsExecutionAllowed(Funcionalidad funcionalidad)
        //{
        //    if (funcionalidad == null) { return true; }
        //    if(IsAdmin()) return true;

        //    Type servicio = GetType();
        //    Funcionalidad? funcionalidadRegistrada = Context.Funcionalidades.FirstOrDefault(f => f.Clase == funcionalidad.Clase && f.Operacion == funcionalidad.Operacion);
        //    if (funcionalidadRegistrada == null) { return true; }

        //    EntidadSecurizada? es = GetEsFuncionalidad(funcionalidadRegistrada);
        //    if(es == null) { return true; }
        //    return Context.HayPermisoEjecucion(CurrentCalipsoforUser?.IDP ?? 0, es.IdEntidadSecurizada);
        //}

        //protected bool IsAccessAllowed(NivelesPrelacion nivel)
        //{
        //    logger.LogInformation(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

        //    if (IsAdmin()) { return true; }
        //    int idp = CurrentCalipsoforUser?.IDP ?? -1;
        //    long idArea = CurrentCalipsoforUser?.IdArea ?? -1;

        //    if (idArea == 0)
        //    {
        //        return CurrentCalipsoforUser == null ? false : Context.Roles.Join(Context.UsuariosRoles, r => r.IdRol, u => u.IdRol, (r, u) => new { r, u })
        //             .Where(z => z.r.Prelacion >= (int)nivel && z.u.IDP == idp).Count() > 0;
        //    }
        //    else
        //    {
        //        return CurrentCalipsoforUser == null ? false : Context.Roles.Join(Context.UsuariosRoles, r => r.IdRol, u => u.IdRol, (r, u) => new { r, u })
        //            .Where(z => z.r.Prelacion >= (int)nivel && z.u.IDP == idp && z.r.IdArea == idArea).Count() > 0;
        //    }
        //}

        //protected EntidadSecurizada? GetEsFuncionalidad(Funcionalidad funcionalidad)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    return Context.EntidadesSecurizadas.AsNoTracking().SingleOrDefault(s => s.IdEntidadReferenciada == funcionalidad.IdFuncionalidad && s.IdTipo == funcionalidad.IdMiTipo);
        //}

        //protected bool IsAdmin()
        //{
        //    logger.LogInformation(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

        //    int IDP = CurrentCalipsoforUser?.IDP ?? 0;
        //    return Context.Roles.Join(Context.UsuariosRoles, r => r.IdRol, u => u.IdRol, (r, u) => new { r, u })
        //        .Where(z => z.r.Prelacion == (int)NivelesPrelacion.Administrador && z.u.IDP == IDP).Any();
        //}

        //protected void ValidaEntidad<T>(T? entidad) where T : CalipsoForModel
        //{

        //    //logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    //if (entidad == null) return;

        //    //Type type = typeof(T);
        //    //IValidador<T>? validador = _registroValidadores.GetValidador<T>(entidad);

        //    //if (validador == null || entidad == null) { return; }

        //    //IEnumerable<string> errores = validador.Valida(entidad);
        //    //if (!errores.IsNullOrEmpty())
        //    //{
        //    //    throw new ValidacionException(type, errores);
        //    //}
        //}

        //protected void ValidaEntidad<T>(T? entidad, IValidador<T> validador) where T : CalipsoForModel
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    if (entidad == null) return;
        //    if (validador == null || entidad == null) { return; }

        //    IEnumerable<string> errores = validador.Valida(entidad);
        //    //if (!errores.IsNullOrEmpty())
        //    //{
        //    //    throw new ValidationException(typeof(T), errores);
        //    //}
        //}
    }
}
