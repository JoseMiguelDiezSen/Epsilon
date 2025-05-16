using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public class ExtensionFuncionalidades : ExtensionAbstracta<EpsilonDbContext>
    {

        public ExtensionFuncionalidades(EpsilonDbContext calypsoDbContext) : base(calypsoDbContext) {
        
        }
    }
}
