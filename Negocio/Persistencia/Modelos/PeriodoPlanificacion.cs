using System.ComponentModel.DataAnnotations.Schema;

namespace Negocio.Persistencia.Modelos
{
    [Table("PeriodosPlanificacion")]
    public class PeriodoPlanificacion
    {
        public long IdPeriodo { get; set; }
        public int IdMiTipo { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public string? PlanesAfectados { get; set; }
        public int Ejercicio { get; set; }
        public int CreadoPorIDP { get; set; }
        public DateTime FechaCreacion { get; set; }
        public long IdArea { get; set;  }
        public decimal Estimado { get; set; }
        public decimal Ejecutado { get; set; }

    }
}
