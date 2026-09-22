using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("Sedes")]
    public class Sede
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSede { get; set; }

        [Required]
        [StringLength(150)]
        public string? NombreSede { get; set; }

        [Required]
        [StringLength(250)]
        public string? DireccionSede { get; set; }

        [Required]
        [StringLength(100)]
        public string? LocalidadSede { get; set; }

        [Required]
        public int TelefonoSede { get; set; }

        [StringLength(100)]
        public string? EMailSede { get; set; }

        [StringLength(100)]
        public string? DirectorSede { get; set; }
    }
}
