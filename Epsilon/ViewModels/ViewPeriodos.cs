using Negocio.Persistencia.Modelos;

namespace Epsilon.ViewModels
{
    public class ViewPeriodos
    {
        public ViewPeriodos() { }

        public ViewPeriodos(DatoPeriodo doEriodato) {
            //IdArea = doEriodato.IdArea;
            idPeriodo = doEriodato.IdPeriodo;
            Ejercicio = doEriodato.Ejercicio;
            //Periodo = doEriodato.Periodo;
            Ejecutado = doEriodato.Ejecutado;
            Estimado = doEriodato.Estimado;
            FechaCreacion = doEriodato.FechaCreacion;
        }

        public long idPeriodo { get; set; }
        public int Ejercicio { get; set; }
        public string? Periodo { get; set; }
        public int Cursos { get; set; }
        public decimal Estimado { get; set; }
        public decimal Ejecutado { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }


        public DateTime FechaCreacion { get; set; }
        //public long IdArea { get; set; }
    }
}
