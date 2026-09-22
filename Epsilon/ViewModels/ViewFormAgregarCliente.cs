using System;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarCliente
    {
        public int IdCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? DNI { get; set; }
        public int Telefono { get; set; }
        public string? EMail { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public DateTime FechaAlta1 { get; set; } = DateTime.Now;
        public int NumeroServicios { get; set; }
        public bool Preferente { get; set; }
        public string? Observaciones { get; set; }
        public string? Comentarios { get; set; }
    }
}
