using Negocio.Persistencia.Modelos;

namespace Negocio.Servicios
{
    public interface IGestionClinica : IServicioEpsilon
    {
        IQueryable <Tratamiento> GetAllTratamientos();
    }
}