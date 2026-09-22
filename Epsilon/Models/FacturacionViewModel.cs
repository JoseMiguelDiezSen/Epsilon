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
        public List<FacturacionChartViewModel> DatosChart { get; set; } // Datos del chart: un registro por cada mes del año actual
        public double TotalFacturacion { get; set; } // Suma de toda la facturación
        public int TotalCitas { get; set; } // Número total de citas facturadas
    }
}
