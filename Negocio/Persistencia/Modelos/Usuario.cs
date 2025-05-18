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
    [Table("Usuarios")]
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string? Nombre { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; } = null;

        public DateTime FechaAlta { get; set; }

        public int Telefono { get; set; }

        public string? RutaFoto { get; set; }

        public bool Activo { get; set; }

     //   public SelectList? TurnoDeTrabajo { get; set; }
    }
}

