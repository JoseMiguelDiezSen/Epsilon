using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosClientes")]
    public class DatosClientes
    {
        public int IdCliente { get; set; }

        public string? NombreCliente { get; set; }

        public string? DNI { get; set; }

        public int Telefono { get; set; }

        public string? EMail { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? Direccion { get; set; }

        public string? Ciudad { get; set; }

        public DateTime FechaAlta { get; set; }

        public int NumeroServicios { get; set; }

        public bool Preferente { get; set; }

        public string? Observaciones { get; set; }

        public DateTime? FechaPrimeraCita { get; set; }

        public DateTime? FechaUltimaCita { get; set; }
    }
}
