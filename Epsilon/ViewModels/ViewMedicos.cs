using Negocio.Persistencia.Modelos;

namespace Epsilon.ViewModels
{
    public class ViewMedicos
    {
        public ViewMedicos(DatosMedicos datosMedico)
        {
            IdMedico = datosMedico.IdMedico;
            NombreMedico = datosMedico.NombreMedico;
            DNI = datosMedico.DNI;
            NumeroColegiado = datosMedico.NumeroColegiado;
            IdUsuario = datosMedico.IdUsuario;
            IdClinica = datosMedico.IdClinica;
            Titulacion = datosMedico.Titulacion;
            Observaciones = datosMedico.Observaciones;
            Activo = datosMedico.Activo;
            FechaContratacion = datosMedico.FechaContratacion;
            EMail = datosMedico.EMail;
            Telefono = datosMedico.Telefono;
            Especialidad = datosMedico.Especialidad;
            NombreClinica = datosMedico.NombreClinica;
        }

        public int IdMedico { get; set; }
        public string? NombreMedico { get; set; }
        public string? DNI { get; set; }
        public int NumeroColegiado { get; set; }
        public int? IdUsuario { get; set; }
        public int IdClinica { get; set; }
        public string? Titulacion { get; set; }
        public string? Observaciones { get; set; }
        public bool Activo { get; set; }
        public string FechaContratacion { get; set; }
        public string? EMail { get; set; }
        public string Telefono { get; set; }
        public string? Especialidad { get; set; }
        public string? NombreClinica { get; set; }
    }
}
