using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Personal")]
    public class Personal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [Column(TypeName = "nchar(10)")]
        public string? FechaContratacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public byte[]? Foto { get; set; }

        [StringLength(150)]
        public string? Titulacion { get; set; }

        /// <summary>
        /// Identificador de la sede o centro de trabajo asignado.
        /// </summary>
        public int? IdSede { get; set; }

        /// <summary>
        /// Identificador de usuario del sistema (si tiene acceso a la aplicación).
        /// </summary>
        public int? IdUsuario { get; set; }
    }
}
