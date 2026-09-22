using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    /// <summary>
    /// Contrato para la gestión y consulta de registros de facturación.
    /// </summary>
    public interface IGestionFacturacion : IServicioEpsilon
    {
        /// <summary>
        /// Obtiene todos los registros de facturación.
        /// </summary>
        IQueryable<Facturacion> GetFacturacion();
    }
}