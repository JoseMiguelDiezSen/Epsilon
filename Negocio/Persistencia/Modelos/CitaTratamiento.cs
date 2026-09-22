using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    /// <summary>
    /// Representa la relación entre una cita médica y el tratamiento asignado.
    /// Tabla: CitaTratamientos
    /// </summary>
    [Table("CitaTratamientos")]
    public class CitaTratamiento
    {
        /// <summary>
        /// Identificador único del registro de tratamiento en la cita.
        /// </summary>
        [Key]
        public int IdCitaTratamiento { get; set; }

        /// <summary>
        /// Identificador de la cita vinculada.
        /// </summary>
        public int? IdCita { get; set; }

        /// <summary>
        /// Identificador del tratamiento aplicado.
        /// </summary>
        public int? IdTratamiento { get; set; }
    }
}
