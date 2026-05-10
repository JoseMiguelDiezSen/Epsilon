using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Utilidades
{
    /// <summary>
    /// Modelo de datos de Reporting Services
    /// </summary>
    public class SSRSSettings
    {
        /// <summary>
        /// Obtiene o establece el servicio de ejecucion
        /// </summary>
        public string? ExecutionService { get; set; }

        /// <summary>
        /// Obtiene o establece el servicio de reportes
        /// </summary>
        public string? ReportingService { get; set; }

        /// <summary>
        /// Obtiene o establece el root del servicio
        /// </summary>
        public string? Root { get; set; }

        /// <summary>
        /// Obtiene o establece el usuario del servicio
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// Obtiene o establece el password de conexion al servicio
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Obtiene o establece el dominio del servicio
        /// </summary>
        public string? Domain { get; set; }
    }
}
