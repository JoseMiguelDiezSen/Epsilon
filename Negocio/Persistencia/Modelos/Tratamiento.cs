using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    [Table("Tratamientos")]
    public class Tratamiento
    {
        [Key]
        public int IdTratamiento { get; set; }

        [Required]
        public string? NombreTratamiento { get; set; }


        public int Precio { get; set; }
    }
}
