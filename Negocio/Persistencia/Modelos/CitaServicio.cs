using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("CitaServicios")]
    public class CitaServicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCitaServicio { get; set; }

        public int IdCita { get; set; }

        public int IdServicio { get; set; }
    }
}
