using Microsoft.EntityFrameworkCore;

namespace Negocio.Servicios
{
    public interface IServicioAbstracto<T> where T : DbContext 
    {
        T Context { get; }
    }
}
