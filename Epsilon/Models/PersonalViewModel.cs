using Epsilon.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Epsilon.Models
{
    public class PersonalViewModel
    {
        public int IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? DNI { get; set; }
        public int NumeroEmpleado { get; set; }
        public string? Puesto { get; set; }
        public string? Telefono { get; set; }
        public string? EMail { get; set; }
        public string? FechaContratacion { get; set; }
        public bool Activo { get; set; }
        public string? Observaciones { get; set; }
        public byte[]? Foto { get; set; }
        public int? IdSede { get; set; }
        public int? IdUsuario { get; set; }
        public string? Titulacion { get; set; }
        public IEnumerable<ViewPersonal> Personal { get; set; } = Enumerable.Empty<ViewPersonal>();
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;
    }
}
