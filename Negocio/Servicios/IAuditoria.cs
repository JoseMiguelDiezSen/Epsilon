using Negocio.Persistencia.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    public interface IAuditoria : IServicioAuditoria
    {
        Usuario? CurrentCalipsoforUser { get; }
    }
}
