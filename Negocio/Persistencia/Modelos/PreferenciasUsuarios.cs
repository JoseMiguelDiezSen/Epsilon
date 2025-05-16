using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    public class PreferenciasUsuarios
    {
        public PreferenciasUsuarios() { }

        public int IdPreferencia { get; set; }

        public int IdUsuario { get; set; }

        public int IdPaciente { get; set; }
    }
}
