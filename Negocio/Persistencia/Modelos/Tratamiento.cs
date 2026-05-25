using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Tratamientos")]
    public class Tratamiento
    {
        public int IdTratamiento { get; set; }

        public string? NombreTratamiento { get; set; }

        public int Duracion { get; set; }

        public string? Color { get; set; }

        public double Precio { get; set; }
    }
}
