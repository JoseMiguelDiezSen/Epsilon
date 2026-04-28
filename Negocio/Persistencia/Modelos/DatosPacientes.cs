using Negocio.Persistencia.Modelos.Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosPacientes")]
    public class DatosPacientes : EpsilonForModel
    {
        /// <summary>
        /// Identificador del paciente.
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del paciente.
        /// </summary>
        public string? NombrePaciente { get; set; }

        /// <summary>
        /// Obtiene o establece el DNI del paciente.
        /// </summary>
        public string? DNI { get; set; }

        /// <summary>
        /// Obtiene o establece el e-mail del paciente.
        /// </summary>
        public int Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de alta del paciente.
        /// </summary>
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Obtiene o establece el numero de consultas del paciente.
        /// </summary>
        public int NumeroConsultas { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electronico del paciente.
        /// </summary>
        public string? EMail { get; set; }

        /// <summary>
        /// Obtiene o establece la ciudad del paciente.
        /// </summary>
        public string? Ciudad { get; set; }

        /// <summary>
        /// Obtiene o establece la direccion del paciente.
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Obtiene o establece si el paciente esta asegurado o no.
        /// </summary>
        public bool Asegurado { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de nacimiento del paciente.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }
    }
}
