using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Negocio.Persistencia.Modelos
{
    [Table("CorreosElectronicos")]
    public class CorreosElectronicos
    {
        /// <summary>
        /// Obtiene o establece el Id del correo electrónico.
        /// </summary>
        public int IdCorreo { get; set; }

        /// <summary>
        /// Onbtiene o establece el nombre del correo electrónico.
        /// </summary>
        public string? NombreCorreo { get; set; }

        /// <summary>
        /// Obtiene o establece el asunto del correo electrónico.
        /// </summary>
        public string? Asunto { get; set; }

        /// <summary>
        /// Obtiene o establece el cuerpo del mensaje del correo electrónico.
        /// </summary>
        public string? CuerpoMensaje { get; set; }



        //public DateTime FechaEnvio { get; set; }
        //public bool Enviado { get; set; }
    }
}
