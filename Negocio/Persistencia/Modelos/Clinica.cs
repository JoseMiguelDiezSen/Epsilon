using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    [Table("Clinicas")]
    public class Clinica
    {
        [Key]
        public int IdClinica { get; set; }

        [Required]
        public string? NombreClinica { get; set; }

        [Required]
        [MaxLength(50)]
        public string? DireccionClinica { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LocalidadClinica { get; set; }

        [Required]
        public int TelefonoClinica { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string? EMailClinica { get; set; }

        public string? DirectorClinica { get; set; }
    }
}
