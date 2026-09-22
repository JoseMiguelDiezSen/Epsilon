using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarPersonal
    {
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(150)]
        public string? NombreEmpleado { get; set; }

        [Required]
        [StringLength(50)]
        public string? DNI { get; set; }

        [Required]
        public int NumeroEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string? Puesto { get; set; }

        [Required]
        [StringLength(50)]
        public string? Telefono { get; set; }

        [Required]
        [StringLength(100)]
        public string? EMail { get; set; }

        [Required]
        public string? FechaContratacion { get; set; }

        public bool Activo { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public IFormFile? FotoSubida { get; set; }

        [StringLength(150)]
        public string? Titulacion { get; set; }

        public int? IdSede { get; set; }

        public int? IdUsuario { get; set; }
    }
}
