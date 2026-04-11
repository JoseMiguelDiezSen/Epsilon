using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    /// <summary>
    /// Esta clase mapea la tabla usuarios de la BBDD y proporciona las propiedades para gestionarlas
    /// </summary>
    [Table("Usuarios")]
    public class Usuario : ProgesforModel
    {
        /// <summary>
        /// Obtiene el identificador unico del usuario
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del usuario
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el password del usuario
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Obtiene o establece el Email del usuario
        /// </summary>
        public string? Email { get; set; } = null;

        /// <summary>
        /// Obtiene o establece la Fecha de Alta del usuario
        /// </summary>
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Obtiene o establece el Telefono del usuario
        /// </summary>
        public int Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece si el usuario esta activo o no
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Foto del usuario en base64
        /// </summary>
        public byte[]? FotoPerfil { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del estado del usuario, que puede ser activo, inactivo, suspendido, etc.
        /// </summary>
        public int IdEstadoUsuario { get; set; }
    }
}

