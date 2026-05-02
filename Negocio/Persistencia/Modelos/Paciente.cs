using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Pacientes")]
    public class Paciente
    {
        /// <summary>
        /// Obtiene el Identificador del Paciente
        /// </summary>
        public int IdPaciente { get; set; }
        
        /// <summary>
        /// Obtiene o establece el nombre del paciente
        /// </summary>
        [Required]
        public string? NombrePaciente { get; set; }

        /// <summary>
        /// Obtiene o establece el DNI del paciente
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? DNI { get; set; }

        /// <summary>
        /// Obtiene o establece el telefono del paciente
        /// </summary>
        [Required]
        public int Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece el email del paciente
        /// </summary>
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string? EMail { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de nacimiento del paciente
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Obtiene o establece la direccion del paciente
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? Direccion { get; set; }

        /// <summary>
        /// Obtiene o establece la ciudad del paciente
        /// </summary>
        public string? Ciudad { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de alta del paciente
        /// </summary>
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Obtiene o establece el numero de consultas del paciente (por año, por mes)?
        /// </summary>
        public int NumeroConsultas { get; set; }

        /// <summary>
        /// Obtiene o establece si el paciente esta asegurado o no
        /// </summary>
        public bool Asegurado { get; set; }

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
    }
}
