using Negocio.Persistencia.Modelos;

namespace Epsilon.ViewModels
{
    public class ViewPersonal
    {
        public ViewPersonal() { }

        public ViewPersonal(DatosPersonal personal)
        {
            IdEmpleado = personal.IdEmpleado;
            NombreEmpleado = personal.NombreEmpleado;
            DNI = personal.DNI;
            NumeroEmpleado = personal.NumeroEmpleado;
            Puesto = personal.Puesto;
            Telefono = personal.Telefono;
            EMail = personal.EMail;
            FechaContratacion = personal.FechaContratacion;
            Activo = personal.Activo;
            Observaciones = personal.Observaciones;
            Foto = personal.Foto;
            IdSede = personal.IdSede;
            IdUsuario = personal.IdUsuario;
            Titulacion = personal.Titulacion;
            NombreSede = personal.NombreSede;
        }

        public int IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? DNI { get; set; }
        public int NumeroEmpleado { get; set; }
        public string? Puesto { get; set; }
        public string? Telefono { get; set; }
        public string? EMail { get; set; }
        public string? FechaContratacion { get; set; }
        public bool Activo { get; set; }
        public string? Observaciones { get; set; }
        public byte[]? Foto { get; set; }
        public int? IdSede { get; set; }
        public int? IdUsuario { get; set; }
        public string? Titulacion { get; set; }
        public string? NombreSede { get; set; }
    }
}
