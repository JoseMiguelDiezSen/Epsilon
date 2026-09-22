using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using System.Linq;

namespace Negocio.Servicios
{
    public interface IGestionServicios
    {
        EpsilonDbContext Context { get; }
        IQueryable<Servicio> GetAllServicios();
        IQueryable<DatosServicios> GetDatosServicios();
        Servicio AddServicio(Servicio servicio);
        Servicio UpdateServicio(Servicio servicio);
        bool DeleteServicio(int idServicio);
        Servicio? GetServicio(int idServicio);
    }
}
