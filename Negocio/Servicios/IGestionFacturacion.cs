using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IGestionFacturacion : IServicioEpsilon
    {
        IQueryable<Facturacion> GetFacturacion();
        void SincronizarFacturaCita(int idCita);
        void EliminarFacturaCita(int idCita);
        void RecalcularFacturacionGlobal();
    }
}
