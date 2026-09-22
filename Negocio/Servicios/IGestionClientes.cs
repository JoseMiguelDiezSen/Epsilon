using Negocio.Persistencia;
using Negocio.Persistencia.Modelos;
using System.Linq;

namespace Negocio.Servicios
{
    public interface IGestionClientes
    {
        EpsilonDbContext Context { get; }
        Cliente AddCliente(Cliente cliente);
        Cliente UpdateCliente(Cliente cliente);
        bool DeleteCliente(int idCliente);
        Cliente GetCliente(int idCliente);
        IQueryable<DatosClientes> GetDatosClientes();
        DatosClientes? GetDetalleCliente(int idCliente);
    }
}
