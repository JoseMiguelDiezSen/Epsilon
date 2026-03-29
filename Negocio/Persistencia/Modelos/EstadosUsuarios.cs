using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("EstadosUsuario")]
    public class EstadosUsuario
    {
        /// <summary>
        /// Obtiene o establece el identificador del estado del usuario
        /// </summary>
        public int IdEstadoUsuario { get; set; }

        /// <summary>
        /// Obtien el estado del usuario
        /// </summary>
        public string? EstadoUsuario { get; private set; }
    }
}
