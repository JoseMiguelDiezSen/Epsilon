using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using System.Linq;

namespace Negocio.Servicios
{
    public interface IGestionPersonal
    {
        EpsilonDbContext Context { get; }
        IQueryable<Personal> GetAllPersonal();
        IQueryable<DatosPersonal> GetDatosPersonal();
        Personal AddPersonal(Personal personal);
        Personal UpdatePersonal(Personal personal);
        bool DeletePersonal(int idEmpleado);
        Personal? GetPersonal(int idEmpleado);
    }
}
