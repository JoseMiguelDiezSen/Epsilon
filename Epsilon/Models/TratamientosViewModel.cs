using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Epsilon.Models
{
    public class TratamientosViewModel
    {
        public int IdTratamiento { get; set; }

        public SelectList? NombreTratamiento { get; set; } = new SelectList(Enumerable.Empty<string>());

        public int Duracion { get; set; }

        public string? Color { get; set; }

        public double Precio { get; set; }

        public IEnumerable<ViewTratamiento> Tratamientos { get; set; } = Enumerable.Empty<ViewTratamiento>();
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;
    }
}
