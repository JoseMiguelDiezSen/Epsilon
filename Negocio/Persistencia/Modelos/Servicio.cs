using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Servicios")]
    public class Servicio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdServicio { get; set; }

        [Required]
        [StringLength(150)]
        public string? NombreServicio { get; set; }

        public int Duracion { get; set; }

        [StringLength(10)]
        public string? Color { get; set; }

        public double Precio { get; set; }
    }
}
