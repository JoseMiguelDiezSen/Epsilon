using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    /// <summary>
    /// Modelo de datos de la tabla Usuarios
    /// </summary>
    /// <remarks>Esta clase mapea la tabla usuarios de la BBDD y proporviona las propiedades para gestionarlas
    /// user data. It includes fields such as the user's name, email, password, registration date, and status.</remarks
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
        /// Obtirene o establece la foto del usuario
        /// </summary>
        public string? RutaFoto { get; set; }

        /// <summary>
        /// Obtiene o establece si el usuario esta activo o no
        /// </summary>
        public bool Activo { get; set; }

     //   public SelectList? TurnoDeTrabajo { get; set; }
    }
}

