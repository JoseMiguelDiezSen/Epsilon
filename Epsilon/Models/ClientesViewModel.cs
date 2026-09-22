using Epsilon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epsilon.Models
{
    public class ClientesViewModel
    {
        public int IdCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? DNI { get; set; }
        public int Telefono { get; set; }
        public string? EMail { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Ciudad { get; set; }
        public DateTime FechaAlta { get; set; }
        public int NumeroServicios { get; set; }
        public bool Preferente { get; set; }
        public IEnumerable<ViewClientes> Clientes { get; set; } = Enumerable.Empty<ViewClientes>();
        public int PaginaActual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 10;

        public DateTime? FechaPrimeraCita { get; set; }
        public DateTime? FechaUltimaCita { get; set; }
        public string? Observaciones { get; set; }
    }
}
