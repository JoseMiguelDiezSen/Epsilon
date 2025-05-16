using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    /// <summary>
    /// Clase encargada de las funcionalidades pertenecientes a Planificacion de periodos y cursos
    /// </summary>
    public class Planificacion : ServicioAbstractoEpsilon, IPlanificacion
    {
        /// <summary>
        /// Interfaz de la clase Seguridad
        /// </summary>
        protected ISeguridad _seguridad;


        /// <summary>
        /// Metodo constructor de la clase
        /// </summary>
        /// <param name="context"></param>
        /// <param name="milogger"></param>
        /// <param name="seguridad"></param>
        public Planificacion(EpsilonDbContext context, ILogger<Planificacion> milogger, ISeguridad seguridad) : base(context, milogger)
        {
            _seguridad = seguridad;            
        }

        #region PERIODOS

        /// <summary>
        /// Metodo para obtener los periodos de un area determinada
        /// </summary>
        /// <returns></returns>
        public IQueryable<DatoPeriodo> GetDatosPeriodos()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosPeriodos;
        }

        //public DatoPeriodo? GetPeriodosPlanificacionByAnio(int? anio)
        //{
        //    //return Context.DatosPeriodos.Where(p => p.Ejercicio == anio).FirstOrDefault();
        //}

        /// <summary>
        /// Metodo para obtener los datos de un periodo concreto
        /// </summary>
        /// <param name="anio">Parametro que indica el año del cual se quieren obtener todos los datos de los Periodos de Planificacion</param>
        /// <returns></returns>
        //public DatoPeriodo? GetDatoPeriodoDeEjercicio(int? anio)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

        //    return Context.DatosPeriodos.Where(p => p.Ejercicio == anio).FirstOrDefault();
        //}

        /// <summary>
        /// Metodo para obtener los periodos de un determinado año
        /// </summary>
        /// <param name="anio">Parametro que indica el año del cual se quieren obtener todos sus Periodos de Planificacion</param>
        /// <returns></returns>
        //public PeriodoPlanificacion? GetPeriodoDeEjercicio(int? anio)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    return Context.PeriodosPlanificacion.Where(p => p.Ejercicio == anio).FirstOrDefault();
        //}

        /// <summary>
        /// Metodo para añadir un periodo de planificacion
        /// </summary>
        /// <param name="periodo">Parametro del modelo de datos de la clase Periodos de Planificacion</param>
        public PeriodoPlanificacion AddPeriodo(PeriodoPlanificacion periodo)
        {
            if (periodo == null) { throw new ArgumentNullException(nameof(periodo)); }

            using (var trans = Context.Database.BeginTransaction())
            {

                Context.SaveChanges();
                trans.Commit();
            }
            return (periodo);
        }

        /// <summary>
        /// Metodo para actualizar los datos de un periodo de planificacion determinado
        /// </summary>
        /// <param name="periodo">Parametro del modelo de datos de la clase PeriodosPlanificacion</param>

        public PeriodoPlanificacion UpdatePeriodo(PeriodoPlanificacion periodo)
        {
            var p = Context.PeriodosPlanificacion.Single(x => x.IdPeriodo == periodo.IdPeriodo);

            p.Ejercicio = periodo.Ejercicio;
            p.Desde = periodo.Desde;
                  
                
            var entity = Context.PeriodosPlanificacion.Update(p);
            
            if (Context.SaveChanges() == 0)
            {
                throw new InvalidOperationException("No se aplicaron los cambios");
            }
            return entity.Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdPeriodo"></param>
        /// <returns></returns>
        public PeriodoPlanificacion DeletePeriodo(long IdPeriodo)
        {
            var p = Context.PeriodosPlanificacion.Single(x => x.IdPeriodo == IdPeriodo);
            var entity = Context.PeriodosPlanificacion.Remove(p);
            Context.SaveChanges();
            return entity.Entity;
        }

        /// <summary>
        /// Metodo para obtener el ejercicio actual en funcion del area que se pase como parametro
        /// </summary>
        /// <param name="idArea">Parametro que indica el identificador del area del cual se quieren obtener el ejercicio actual</param>
        //public int? GetEjercicioActual(long idArea)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

        //    return Context.PeriodosPlanificacion.Where(x => x.IdArea == idArea).OrderByDescending(x => x.Ejercicio).Select(p => p.Ejercicio).FirstOrDefault();
        //}

        /// <summary>
        /// Comprueba si un Periodo de Planificación esta abierto
        /// </summary>
        /// <param name="idPeriodo">Identificador del Periodo de Planificación</param>
        /// <returns>Valor booleano que indica si el Periodo está abierto (true) o cerrado (false)</returns>
        //public bool EsPeriodoAbierto(long idPeriodo)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

        //    PeriodoPlanificacion periodo = Context.PeriodosPlanificacion.Single(p => p.IdPeriodo == idPeriodo);
        //    var hoy = DateTime.Now;
        //    return periodo.Desde <= hoy && periodo.Hasta >= hoy;
        //}

        ///// <summary>
        ///// Metodo para obtener los ejercicios en funcion del idArea que se pase como parametro
        ///// </summary>
        ///// <param name="idArea">Parametro que indica el identificador del area del cual se quieren obtener todos los ejercicios</param>
        //public IEnumerable<PeriodoPlanificacion> GetPeriodosEnArea(long idArea)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);

        //    var ejercicios = Context.PeriodosPlanificacion.Where(x => x.IdArea == idArea).ToList();
        //    return ejercicios;
        //}

        //PeriodoPlanificacion? GetPeriodo(long? id)
        //{
        //    return Context.PeriodosPlanificacion.SingleOrDefault(p => p.IdPeriodo == id);
        //}

        ///// <summary>
        ///// Método para obtener el último Periodo Planificado
        ///// </summary>
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //public virtual PeriodoPlanificacion? GetPeriodo(long idPeriodo)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    return Context.PeriodosPlanificacion.SingleOrDefault(p=> p.IdPeriodo == idPeriodo);
        //}

        ///// <summary>
        ///// Método para obtener los datos de un Periodo Planificado
        ///// </summary>
        ///// <param name="idPeriodo">Identificador del Periodo</param>
        ///// <returns></returns>
        //public DatoPeriodo? GetDatoPeriodo(long idPeriodo)
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    return Context.DatosPeriodos.SingleOrDefault(p=> p.IdPeriodo == idPeriodo);
        //}

        ///// <summary>
        ///// Método para obtener el último Periodo Planificado
        ///// </summary>
        //public PeriodoPlanificacion? GetUltimoPeriodo()
        //{
        //    logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
        //    return Context.PeriodosPlanificacion.OrderByDescending(x => x.Ejercicio).FirstOrDefault();
        //}
        #endregion
    }
}
