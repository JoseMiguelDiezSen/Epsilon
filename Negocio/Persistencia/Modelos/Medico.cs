using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    [Table("Medicos")]
    public class Medico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMedico { get; set; }

        [Required]
        [StringLength(50)]
        public string? NombreMedico { get; set; }

        [Required]
        [StringLength(50)]
        public string? DNI { get; set; }

        [Required]
        public int NumeroColegiado { get; set; }

        [Required]
        [StringLength(50)]
        public string? Especialidad { get; set; }

        [Required]
        [StringLength(50)]
        public string? Telefono { get; set; }

        [Required]
        [StringLength(50)]
        public string? EMail { get; set; }

        [Required]
        [Column(TypeName = "nchar(10)")]
        public string? FechaContratacion { get; set; }

        [Required]
        public bool Activo { get; set; }

        [StringLength(1)]
        public string? Observaciones { get; set; }

        // Para almacenar la foto en formato binario (varbinary(100))
        public byte[]? Foto { get; set; }



    }
}
