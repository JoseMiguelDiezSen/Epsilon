using Epsilon.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Models
{
    public class PlanificacionPeriodosViewModel
    {
        public int Ejercicio { get; set; }
        public string? Plan { get; set; }
        public decimal Estimado { get; set; }
        public long IdArea { get; set; }

        public DateTime FechaCreacion { get; set; }
        public decimal Ejecutado { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 5;
        public IEnumerable<ViewPeriodos> Periodos { get; set; } = Enumerable.Empty<ViewPeriodos>();
    }
}
