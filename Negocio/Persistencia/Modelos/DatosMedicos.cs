using Negocio.Persistencia.Modelos.Comun;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosMedicos")]
    public class DatosMedicos : EpsilonForModel
    {
        /// <summary>
        /// Obtiene o establece el identificador del médico.
        /// </summary>
        public int IdMedico { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del médico.
        /// </summary>
        public string? NombreMedico { get; set; }

        /// <summary>
        /// Obtiene o establece el DNI del médico.
        /// </summary>
        public string? DNI { get; set; }

        /// <summary>
        /// Obtiene o establece el número de colegiado del médico.
        /// </summary>
        public int NumeroColegiado { get; set; }
        
        /// <summary>
        /// Obtiene o establece el identificador del usuario asociado al médico.
        /// </summary>
        public int? IdUsuario { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la clínica asociada al médico.
        /// </summary>
        public int IdClinica { get; set; }

        /// <summary>
        /// Obtiene o establece la titulación del médico.
        /// </summary>
        public string? Titulacion { get; set; }
        
        /// <summary>
        /// Obtiene o establece las observaciones del médico.
        /// </summary>
        public string? Observaciones { get; set; }

        /// <summary>
        /// Obtiene o establece si el médico está activo o no.
        /// </summary>
        public bool Activo { get; set; }
        
        /// <summary>
        /// Obtiene o establece la fecha de contratación del médico.
        /// </summary>
        public string FechaContratacion { get; set; }

        /// <summary>
        /// Obtiene o establece el correo electrónico del médico.
        /// </summary>
        public string? EMail { get; set; }

        /// <summary>
        /// Obtiene o establece el número de teléfono del médico.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Obtitene o establece la especialidad del médico.
        /// </summary>
        public string? Especialidad { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la clínica asociada.
        /// </summary>
        public string? NombreClinica { get; set; }
    }
}
