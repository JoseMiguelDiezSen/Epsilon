using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Citas")]
    public class Citas
    {
        public int IdCita { get; set; }

        public int IdClinica { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int IdPaciente { get; set; }

        public int IdMedico { get; set; }

        public string? Observaciones { get; set; }
    }
}
