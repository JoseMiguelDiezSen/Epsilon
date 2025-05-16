using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public abstract class ExtensionAbstracta<T> where T : DbContext
    {
        protected ExtensionAbstracta(T context) {

            Context = context;
  
        }

        public T Context { internal get; set; }

    }
}
