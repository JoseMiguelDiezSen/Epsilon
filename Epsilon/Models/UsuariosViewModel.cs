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

        public string? Password { get; set; }

        public string? Email { get; set; }

        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public int Telefono { get; set; }

        public string? RutaFoto { get; set; }

        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 5;

        public IEnumerable<ViewUsuario> Usuarios { get; set; } = Enumerable.Empty<ViewUsuario>();
    }
}
