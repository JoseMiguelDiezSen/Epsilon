using Epsilon.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Epsilon.Models
{
    public class ServiciosViewModel
    {
        public int IdServicio { get; set; }
        public string? NombreServicio { get; set; }
        public int Duracion { get; set; }
        public string? Color { get; set; }
        public double Precio { get; set; }
        public IEnumerable<ViewServicio> Servicios { get; set; } = Enumerable.Empty<ViewServicio>();
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;
    }
}
