using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;
using System.Reflection;

namespace Negocio.Servicios
{
    public class GestionMedicos : ServicioAbstractoEpsilon, IGestionMedicos
    {
        /// <summary>
        /// Constructor del servicio
        /// </summary>
        /// <param name="context"></param>
        public GestionMedicos(EpsilonDbContext context, ILogger<GestionMedicos> logger, IValidadoresProgesfor registroValidadores) : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        public IQueryable<DatosMedicos> GetDatosMedicos()
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name);
            return Context.DatosMedicos;
        }
    }
}
