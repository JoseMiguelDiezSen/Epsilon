using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Persistencia.Modelos.Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Persistencia.Modelos
{
    ///<summary> Modelo de datos de la vista [vDatosUsuario]. </summary>
    [Table("vDatosUsuarios")]
    public class DatosUsuario : EpsilonForModel
    {
        /// <summary> Identificador del usuario. </summary>
        public int IdUsuario { get; set; }

        /// <summary> Obtiene o establece el nombre del usuario. </summary>
        public string? Nombre { get; set; }

        /// <summary> Obtiene o establece el password del usuario. </summary>
        public string? Password { get; set; }

        /// <summary> Obtiene o establece el e-mail del usuario. </summary>
        public string? EMail { get; set; }

        /// <summary> Obtiene o establece la fecha de alta del usuario. </summary>
        public DateTime FechaAlta { get; set; }

        /// <summary> Obtiene o establece el telefono del usuario. </summary>
        public int Telefono { get; set; }

        /// <summary> Obtiene o establece la foto del usuario. </summary>
        public string? RutaFoto { get; set; }

        public bool Activo { get; set; }

        //public SelectList? TurnoDeTrabajo { get; set; }
    }
}
