using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    /// <summary>
    /// Representa un hueco o franja horaria en la agenda de un médico.
    /// Tabla: Agenda
    /// </summary>
    [Table("Agenda")]
    public class Agenda
    {
        /// <summary>
        /// Identificador único del registro en agenda.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgenda { get; set; }

        /// <summary>
        /// Identificador del médico asignado a esta franja.
        /// </summary>
        public int IdMedico { get; set; }

        /// <summary>
        /// Fecha y hora de inicio de la franja o cita.
        /// </summary>
        public DateTime HoraInicio { get; set; }

        /// <summary>
        /// Fecha y hora de fin de la franja o cita.
        /// </summary>
        public DateTime HoraFin { get; set; }

        /// <summary>
        /// Estado de disponibilidad del hueco:
        /// 1 (true) = Disponible / libre
        /// 0 (false) = Ocupado por una cita
        /// </summary>
        public bool Disponible { get; set; }

        /// <summary>
        /// Identificador de la cita asociada cuando el hueco está ocupado (opcional).
        /// </summary>
        public int? IdCita { get; set; }
    }
}
