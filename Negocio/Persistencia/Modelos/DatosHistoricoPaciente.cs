using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosHistorialPaciente")]
    public class DatosHistoricoPaciente
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
        /// Obtiene o establece la fecha de alta del paciente.
        /// </summary>
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Obtiene o establece el numero de consultas del paciente.
        /// </summary>
        public int NumeroConsultas { get; set; }


 




        /// <summary>
        /// Obtiene o establece las observaciones.
        /// </summary>
        public string? Observaciones { get; set; }




        /// <summary>
        /// Obtiene el identificador de la cita
        /// </summary>
        public int IdCita { get; set; }


        /// <summary>
        /// Obtiene o establece la fecha de Inicio de la cita
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de fin de la cita
        /// </summary>
        public DateTime FechaFin { get; set; }



        /// <summary>
        /// Obtiene o establece la descripción de la condición bucal del paciente.
        /// </summary>
        public string NombreMedico { get; set; }



        /// <summary>
        /// Obtiene o establece la descripción de la condición bucal del paciente.
        /// </summary>
        public string NombreClinica { get; set; }
    }
}
