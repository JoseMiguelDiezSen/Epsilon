using Negocio.Persistencia.Modelos.Comun;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosTratamientos")]
    public class DatosTratamientos : EpsilonForModel
    {
        /// <summary>
        /// Obtiene o establece el identificador del tratamiento.
        /// </summary>
        public int IdTratamiento { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del tratamiento.
        /// </summary>
        public string? NombreTratamiento { get; set; }

        /// <summary>
        /// Obtiene o establece la duracion del tratamiento.
        /// </summary>
        public int Duracion { get; set; }

        /// <summary>
        /// Ontiene o establece el Precion del tratamiento.
        /// </summary>
        public double Precio { get; set; }

        /// <summary>
        /// Obtiene o establece el color del tratamiento.
        /// </summary>
        public string? Color { get; set; }
    }
}
