using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Radiologia")]
    public class Radiografia
    {
        public int IdRadiografia { get; set; }

        public int IdPaciente { get; set; }

        public string? Archivo { get; set; }

        public DateTime FechaArchivo { get; set; }

        public string? Tipo { get; set; }

        public string? Observaciones { get; set; }

        // Navegación
        public Paciente? Paciente { get; set; }
    }
}
