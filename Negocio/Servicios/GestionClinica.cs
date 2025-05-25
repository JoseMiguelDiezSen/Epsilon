using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;

namespace Negocio.Servicios
{
    public class GestionClinica : ServicioAbstractoEpsilon
    {
        public GestionClinica(EpsilonDbContext context, ILogger <GestionUsuarios> logger, ISeguridad seguridad) : base(context, logger)
        {
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        public IEnumerable<Tratamiento> GetAllTratamientos()
        {
            var tratamientos = Context.Tratamientos.ToList();
            return tratamientos;
        }
    }
}
