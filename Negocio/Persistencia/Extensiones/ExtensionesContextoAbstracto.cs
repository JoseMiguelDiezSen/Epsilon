using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public abstract class ExtensionesContextoAbstracto<T> where T : DbContext
    {

        protected T Context { get; set; }
        
        public ExtensionesContextoAbstracto(T context)
        {
            Context = context;
            RegistrarExtensiones();
        }

        public abstract void RegistrarExtensiones();


    }
}
