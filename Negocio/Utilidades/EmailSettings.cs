using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Utilidades
{
    /// <summary>
    /// Modelo de datos de las opciones de mail
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Obtiene o establece el servidor a utilizar
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// Obtiene o establece el puerto a utilizar
        /// </summary>
        public string? Port { get; set; }

        /// <summary>
        /// Obtiene o establece el emisor del mail
        /// </summary>
        public string? From { get; set; }

        /// <summary>
        /// Obtiene o establece el receptor del mail
        /// </summary>
        public string? ReplyTo { get; set; }

        /// <summary>
        /// Obtiene o establece el usuario del mail
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// Obtiene o establece el password
        /// </summary>
        public string? Password { get; set; }
    }

}
