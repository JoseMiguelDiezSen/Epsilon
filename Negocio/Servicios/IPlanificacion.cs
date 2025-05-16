using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Negocio.Servicios.Comun;
using System.Numerics;

namespace Negocio.Servicios
{
    /// <summary>
    /// Interfaz correspondiente a la clase Planificacion. En ella se definen los metodos existentes en dicha clase.
    /// </summary>
    public interface IPlanificacion : IServicioEpsilon
    {
        /// <summary>
        /// Método que obtiene el Periodo Indicado.
        /// </summary>
        /// <param name="idPeriodo">Identificador del Periodo a obtener</param>
        /// <returns></returns>
        //PeriodoPlanificacion? GetPeriodo(long idPeriodo);

        /// <summary>
        /// Método que obtiene el Periodo Indicado.
        /// </summary>
        /// <param name="idPeriodo">Identificador del Periodo a obtener</param>
        /// <returns></returns>
        //DatoPeriodo? GetDatoPeriodo(long idPeriodo);

        /// <summary>
        /// Metodo que contiene la funcionalidad para obtener los datos de los periodos de in area determinada por parametro
        /// </summary>
        /// <param name="idArea"></param>
        /// <returns></returns>
        IQueryable<DatoPeriodo> GetDatosPeriodos();

        /// <summary>
        /// Metodo que contiene la funcionalidad para obtener los datos de los periodos de un año determinado
        /// </summary>
        /// <param name="anio">Parametro de entrada para indicar el año del cual se quieren obtener los datos de los periodos</param>
        /// <returns></returns>
        //DatoPeriodo? GetDatoPeriodoDeEjercicio(int? anio);


        //DatoPeriodo? GetPeriodosPlanificacionByAnio(int? anio);
        /// <summary>
        /// Obtiene el último Periodo Planificado
        /// </summary>
        /// <returns>El último Periodo</returns>
        //PeriodoPlanificacion? GetUltimoPeriodo();

        /// <summary>
        /// Metodo que contiene la funcionalidad necesaria para obtener los periodos de planificacion pertenecientes a un año especifico
        /// </summary>
        /// <param name="anio">Parametro de entrada que contiene el año del cual se quiere obtener los periodos planificados</param>
        /// <returns></returns>
        ////PeriodoPlanificacion? GetPeriodoDeEjercicio(int? anio);

        /// <summary>
        /// Método que contiene la funcionalidad necesaria para actualizar un periodo de planificacion
        /// </summary>
        /// <param name="periodo"></param>
        /// <returns></returns>
        PeriodoPlanificacion UpdatePeriodo(PeriodoPlanificacion periodo);

        /// <summary>
        /// Método que contiene la funcionalidad necesaria para eliminar un periodo de planificacion  
        /// </summary>
        /// <param name="IdPeriodo">Variable que contiene la funcionalidad correspondiente a la eliminacion de un periodo de planificacion</param>
        /// <returns></returns>
        PeriodoPlanificacion DeletePeriodo(long IdPeriodo);

        /// <summary>
        /// Metodo que contiene la funcionalidad correspondiente al proceso de añadir un periodo de planificacion
        /// </summary>
        /// <param name="element">Contiene el modelo de datos de la clase periodos de planificacion</param>
        /// <returns></returns>
        PeriodoPlanificacion AddPeriodo(PeriodoPlanificacion element);
    }
}






