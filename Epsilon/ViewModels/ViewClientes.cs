using Negocio.Persistencia.Modelos;
using System;

namespace Epsilon.ViewModels
{
    public class ViewClientes
    {
        public ViewClientes() { }

        public ViewClientes(DatosClientes cliente)
        {
            IdCliente = cliente.IdCliente;
            NombreCliente = cliente.NombreCliente;
            DNI = cliente.DNI;
            Telefono = cliente.Telefono;
            EMail = cliente.EMail;
            FechaNacimiento = cliente.FechaNacimiento;
            Direccion = cliente.Direccion;
            Ciudad = cliente.Ciudad;
            FechaAlta = cliente.FechaAlta;
            NumeroServicios = cliente.NumeroServicios;
            Preferente = cliente.Preferente;
            Observaciones = cliente.Observaciones;
        }

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
    }
}
