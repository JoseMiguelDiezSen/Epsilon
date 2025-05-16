using Microsoft.EntityFrameworkCore.ChangeTracking;
using Negocio.Persistencia.Modelos;
using Negocio.Persistencia.Modelos.Comun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public class ExtensionEntidadesSecurizadas : ExtensionAbstracta<EpsilonDbContext>
    {
        public ExtensionEntidadesSecurizadas(EpsilonDbContext context) : base(context) {   
        }
    }
}
