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
        public string? Server { get; set; } = "smtp.gmail.com";

        /// <summary>
        /// Obtiene o establece el puerto a utilizar
        /// </summary>
        public int? Port { get; set; } = 587;

        /// <summary>
        /// Obtiene o establece el emisor del mail
        /// </summary>
        public string? From { get; set; } = "jsm198969@gmail.com";

        /// <summary>
        /// Obtiene o establece el receptor del mail
        /// </summary>
        public string? ReplyTo { get; set; } = "noResponder@gmail.com";

        /// <summary>
        /// Obtiene o establece el usuario del mail
        /// </summary>
        public string? User { get; set; } = "jsm198969@gmail.com";

        /// <summary>
        /// Obtiene o establece el password de la app para el envio de correo.
        /// </summary>
        public string? PasswordApp { get; set; } = "prlrnmctuqrnpdgu";
    }
}
