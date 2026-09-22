namespace Epsilon.Models
{
    public class FacturacionViewModel
    {
        public int IdCita { get; set; }
        public string NombrePaciente { get; set; }
        public string NombreMedico { get; set; }
        public string NombreTratamiento { get; set; }
        public double Precio { get; set; }
        public DateTime FechaCita { get; set; }
        public DateTime FechaFactura { get; set; }
        public double TotalFacturacion { get; set; }
        public int TotalCitas { get; set; }
    }
}
