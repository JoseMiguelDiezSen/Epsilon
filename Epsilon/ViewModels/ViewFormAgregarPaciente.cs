namespace Epsilon.ViewModels
{
    public class ViewFormAgregarPaciente
    {
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
