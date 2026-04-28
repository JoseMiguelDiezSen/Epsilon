using Negocio.Persistencia.Modelos;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Epsilon.ViewModels
{
    public class ViewPacientes
    {
        public ViewPacientes(DatosPacientes datosPaciente) {
            IdPaciente = datosPaciente.IdPaciente;
            NombrePaciente = datosPaciente.NombrePaciente;
            DNI = datosPaciente.DNI;
            Telefono = datosPaciente.Telefono;
            Direccion = datosPaciente.Direccion;
            Ciudad = datosPaciente.Ciudad;
            EMail = datosPaciente.EMail;
            FechaAlta = datosPaciente.FechaAlta;
            NumeroConsultas = datosPaciente.NumeroConsultas;
            Asegurado = datosPaciente.Asegurado;
        }

        public int IdPaciente { get; set; }
        public string? NombrePaciente { get; set; }
        public string? DNI { get; set; }
        public int Telefono { get; set; }
        public string? EMail { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Ciudad { get; set; }
        public DateTime FechaAlta { get; set; }
        public int NumeroConsultas { get; set; }
        public bool Asegurado { get; set; }
    }
}
