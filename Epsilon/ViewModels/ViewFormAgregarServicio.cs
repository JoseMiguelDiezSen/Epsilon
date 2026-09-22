using System.ComponentModel.DataAnnotations;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarServicio
    {
        public int IdServicio { get; set; }

        [Required]
        [StringLength(150)]
        public string? NombreServicio { get; set; }

        public int Duracion { get; set; } = 30;

        public string? Color { get; set; } = "#3788d8";

        public double Precio { get; set; }
    }
}
