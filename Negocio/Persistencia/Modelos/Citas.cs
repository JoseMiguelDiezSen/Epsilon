using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Citas")]
    public class Citas
    {
        /// <summary>
        /// Identificador único de la cita.
        /// </summary>
        public int IdCita { get; set; }

        /// <summary>
        /// Identificador de la clínica donde se llevará a cabo la cita.
        /// </summary>
        public int IdClinica { get; set; }

        /// <summary>
        /// Fecha y Hora de inicio de la cita.
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha y Hora de fin de la cita.
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Identificador del paciente asociado a la cita.
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Identificador del médico asociado a la cita.
        /// </summary>
        public int IdMedico { get; set; }

        /// <summary>
        /// Observaciones adicionales relacionadas con la cita.
        /// </summary>
        public string? Observaciones { get; set; }
    }
}
