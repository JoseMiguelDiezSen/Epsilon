using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;

namespace Negocio.Servicios
{
    /// <summary>
    /// Servicio que gestiona la consulta de registros de facturación.
    /// </summary>
    public class GestionFacturacion : ServicioAbstractoEpsilon, IGestionFacturacion
    {
        public GestionFacturacion(EpsilonDbContext context, ILogger<GestionFacturacion> logger, IValidadoresProgesfor registroValidadores)
            : base(context, logger, registroValidadores)
        {
            logger.LogTrace(GetEventId(), "Servicio GestionFacturacion iniciado");
        }

        /// <summary>
        /// Obtiene la lista completa de registros de facturación.
        /// </summary>
        public IQueryable<Facturacion> GetFacturacion()
        {
            return Context.Facturacion.AsQueryable(); // Devuelve todos los registros de facturación para poder sumar por mes
        }
    }
}