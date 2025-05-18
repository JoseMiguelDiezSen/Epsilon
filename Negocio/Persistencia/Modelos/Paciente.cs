using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    [Table("Pacientes")]
    public class Paciente
    {
        [Key]
        public int IdPaciente { get; set; }

        [Required]
        public string? NombrePaciente { get; set; }

        [Required]
        [MaxLength(50)]
        public string? DireccionPaciente { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LocalidadPaciente { get; set; }

        [Required]
        public int TelefonoPaciente { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string? EMailPaciente { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}
