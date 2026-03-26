using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    public class Citas
    {
        public int IdCita { get; set; }

        public int IdClinica { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int IdPaciente { get; set; }

        public int IdMedico { get; set; }

        public string? Observaciones { get; set; }
    }
}
