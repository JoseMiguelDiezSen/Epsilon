namespace Epsilon.Models
{
    public class FacturacionChartViewModel
    {
        public string Periodo { get; set; } // Mes en formato yyyy-MM (ej: 2026-01)
        public double TotalFacturacion { get; set; } // Suma de importes facturados en ese mes
        public int NumeroCitas { get; set; } // Nº de citas facturadas en ese mes
    }
}