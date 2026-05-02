using Epsilon.ViewModels;

namespace Epsilon.Models
{
    public class PacientesViewModel
    {
        public int IdPaciente { get; set; }
        public string? NombrePaciente { get; set; }
        public string? DNI { get; set; }
        public int Telefono { get; set; }
        public string? EMail { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Ciudad { get; set; }
        public DateTime FechaAlta { get; set; }
        public int NumeroConsultas { get; set; }
        public bool Asegurado { get; set; }
        public IEnumerable<ViewPacientes> Pacientes { get; set; } = Enumerable.Empty<ViewPacientes>();
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;

        /// <summary>
        /// Obtiene o establece la fecha de alta del paciente.
        /// </summary>
        public DateTime FechaPrimeraCita { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de alta del paciente.
        /// </summary>
        public DateTime FechaUltimaCita { get; set; }

        /// <summary>
        /// Obtiene o establece las observaciones.
        /// </summary>
        public string? Observaciones { get; set; }


        /// <summary>
        /// Obtiene o establece alergias asociadas al individuo.
        /// </summary>
        public string? Alergias { get; set; }

        /// <summary>
        /// Obtiene o establece si el paciente es fumador o no
        /// </summary>
        public bool Fumador { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la condición bucal del paciente.
        /// </summary>
        public string? CondicionBucal { get; set; }
    }
}
