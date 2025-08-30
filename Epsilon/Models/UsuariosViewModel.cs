using Epsilon.ViewModels;

namespace Epsilon.Models
{
    /// <summary>
    /// Modelo de datos para Usuario
    /// </summary>
    public class UsuariosViewModel
    {
        /// <summary>
        /// Obtiene o establece el nombre del usuario
        /// </summary>
        public string? Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el password de un usuario
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Obtiene o establece el email de un usuario
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de alta de un usuario
        /// </summary>
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        /// <summary>
        /// Obtiene o establece el telefono de un usuario
        /// </summary>
        public int Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece la foto de un usuario
        /// </summary>
        public string? RutaFoto { get; set; }

        /// <summary>
        /// Obtiene o establece la pagina actual de la tabla
        /// </summary>
        public int PaginaActual { get; set; } = 1;

        /// <summary>
        /// Obtiene o establece el numero de registros por pagina
        /// </summary>
        public int RegistrosPorPagina { get; set; } = 5;

        /// <summary>
        /// Obtiene o establece el nombre del usuario
        /// </summary>
        public IEnumerable<ViewUsuario> Usuarios { get; set; } = Enumerable.Empty<ViewUsuario>();
    }
}
