using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Servicios
{
    public class Auditoria : ServicioAbstracto <AuditoriaDbContext>, IAuditoria
    {
        public Auditoria(AuditoriaDbContext context, ILogger<Auditoria> milogger) : base(context, milogger) { 
           
        }
    }
}
