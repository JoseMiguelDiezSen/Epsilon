using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosServicios")]
    public class DatosServicios
    {
        public int IdServicio { get; set; }

        public string? NombreServicio { get; set; }

        public int Duracion { get; set; }

        public string? Color { get; set; }

        public double Precio { get; set; }
    }
}
