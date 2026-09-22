using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("vDatosHistorialCliente")]
    public class DatosHistoricoCliente
    {
        public int IdCliente { get; set; }

        public string? NombreCliente { get; set; }

        public string? DNI { get; set; }

        public DateTime FechaAlta { get; set; }

        public int NumeroServicios { get; set; }

        public int IdCita { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string? Observaciones { get; set; }

        public string? NombreEmpleado { get; set; }

        public string? NombreSede { get; set; }

        public string? NombreServicio { get; set; }

        public double Precio { get; set; }

        public int Duracion { get; set; }
    }
}
