using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    public interface IServicioAbstracto<T> where T : DbContext 
    {
        T Context { get; }
    }
}
