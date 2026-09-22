using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Facturacion")]
    public class Facturacion
    {
        [Key]
        public int IdFactura { get; set; }

        public double Importe { get; set; }

        public DateTime FechaFactura { get; set; }

        public int IdCita { get; set; }
    }
}
