using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    [Table("PermisoRolTipoEntidad")]
    public class PermisoRolTipoEntidad
    {
        public long IdPermisoTipo { get; set; }

        public int IdTipoEntidad { get; set; }
        public long IdRol { get; set; }
        public bool ConsultaConcedida { get; set; }
        public bool ModificacionConcedida { get; set; }
        public bool EliminacionConcedida { get; set; }
        public bool EjecucionConcedida { get; set; }
        public bool ConsultaDenegada { get; set; }
        public bool ModificacionDenegada { get; set; }
        public bool EliminacionDenegada { get; set; }
        public bool EjecucionDenegada { get; set; }
    }
}
