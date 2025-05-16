using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Extensiones
{
    public class ExtensionesEpsilon : ExtensionesContextoAbstracto<EpsilonDbContext>
    {
        public ExtensionesEpsilon(EpsilonDbContext calypsoDbContext) : base(calypsoDbContext) {
        } 

        public override void RegistrarExtensiones()
        {
            Permisos = new ExtensionPermisos(Context);
            Usuarios = new ExtensionUsuarios(Context);
            Funcionalidades = new ExtensionFuncionalidades(Context);
            Roles = new ExtensionRoles(Context);
        }

        public virtual ExtensionPermisos Permisos { get; set; }
        public virtual ExtensionUsuarios Usuarios { get; set; }
        public virtual ExtensionFuncionalidades Funcionalidades { get; set; }
        public virtual ExtensionRoles Roles { get; set; }
        public virtual ExtensionEntidadesSecurizadas EntidadesSecurizadas { get; set; }
    }

}
