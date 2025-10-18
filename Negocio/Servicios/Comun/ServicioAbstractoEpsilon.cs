using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Negocio.Excepciones;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Validadores.Comun;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Negocio.Servicios.Comun
{
    public abstract class ServicioAbstractoEpsilon : ServicioAbstracto<EpsilonDbContext>, IServicioEpsilon
    {
        protected IValidadoresProgesfor _registroValidadores;

        public ServicioAbstractoEpsilon(EpsilonDbContext context, ILogger milogger, IValidadoresProgesfor registroValidadores) : base(context, milogger)
        {
            _registroValidadores = registroValidadores;
        }

        /// <summary>
        /// Valida una entidad 
        /// </summary>
        /// <typeparam name="T"> Parametro de entrada de la entidad a validar </typeparam>
        /// <param name="entidad"></param>
        /// <exception cref="ValidacionException"></exception>
        protected virtual void ValidaEntidad<T>(T? entidad) where T : ProgesforModel
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            if (entidad == null) return;

            Type type = typeof(T);
            IValidador<T>? validador = _registroValidadores.GetValidador<T>(entidad);

            if (validador == null || entidad == null) { return; }

            IEnumerable<string> errores = validador.Valida(entidad);
            if (!errores.IsNullOrEmpty())
            {
                throw new ValidacionException(type, errores);
            }
        }

        /// <summary>
        /// Metodo encargado de validar una entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entidad">Parametro de entrada de la entidad a validar </param>
        /// <param name="validador"></param>
        /// <exception cref="ValidationException"></exception>
        protected virtual void ValidaEntidad<T>(T? entidad, IValidador<T> validador) where T : EpsilonDbContext
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            if (entidad == null) return;
            if (validador == null || entidad == null) { return; }

            IEnumerable<string> errores = validador.Valida(entidad);
            if (!errores.IsNullOrEmpty())
            {
                throw new ValidacionException(typeof(T), errores);
            }
        }

        /// <summary>
        /// Metodo encargado de validar una entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entidad">Parametro de entrada de la entidad a validar </param>
        /// <param name="operacion"></param>
        /// <exception cref="ValidacionException"></exception>
        protected virtual void ValidaEntidad<T>(T? entidad, string operacion) where T : ProgesforModel
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            if (entidad == null) return;

            Type type = typeof(T);
            IValidador<T>? validador = _registroValidadores.GetValidador<T>(entidad, operacion);

            if (validador == null || entidad == null) { return; }

            IEnumerable<string> errores = validador.Valida(entidad);
            if (!errores.IsNullOrEmpty())
            {
                throw new ValidacionException(type, errores);
            }
        }
    }
}
