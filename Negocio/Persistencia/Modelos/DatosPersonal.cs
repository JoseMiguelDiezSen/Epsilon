using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosPersonal")]
    public class DatosPersonal
    {
        public int IdEmpleado { get; set; }

        public string? NombreEmpleado { get; set; }

        public string? DNI { get; set; }

        public int NumeroEmpleado { get; set; }

        public int? IdUsuario { get; set; }

        public int? IdSede { get; set; }

        public string? Titulacion { get; set; }

        public byte[]? Foto { get; set; }

        public string? Observaciones { get; set; }

        public bool Activo { get; set; }

        public string? FechaContratacion { get; set; }

        public string? EMail { get; set; }

        public string? Telefono { get; set; }

        public string? Puesto { get; set; }

        public string? NombreSede { get; set; }
    }
}
