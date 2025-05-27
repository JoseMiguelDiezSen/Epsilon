using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;

namespace Negocio.Servicios
{
    public class GestionClinica : ServicioAbstractoEpsilon, IGestionClinica
    {
        public GestionClinica(EpsilonDbContext context, ILogger <GestionUsuarios> logger, ISeguridad seguridad) : base(context, logger)
        {
            logger.LogTrace(GetEventId(), "Servicion iniciado");
        }

        public virtual IQueryable<Tratamiento> GetAllTratamientos()
        {
            var tratamientos = Context.Tratamientos.ToList();
            return Context.Tratamientos.AsNoTracking();
        }
    }
}
