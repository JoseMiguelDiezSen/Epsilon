using Negocio.Persistencia.Modelos;

namespace Epsilon.ViewModels
{
    public class ViewServicio
    {
        public ViewServicio() { }

        public ViewServicio(DatosServicios servicio)
        {
            IdServicio = servicio.IdServicio;
            NombreServicio = servicio.NombreServicio;
            Duracion = servicio.Duracion;
            Color = servicio.Color;
            Precio = servicio.Precio;
        }

        public ViewServicio(Servicio servicio)
        {
            IdServicio = servicio.IdServicio;
            NombreServicio = servicio.NombreServicio;
            Duracion = servicio.Duracion;
            Color = servicio.Color;
            Precio = servicio.Precio;
        }

        public int IdServicio { get; set; }
        public string? NombreServicio { get; set; }
        public int Duracion { get; set; }
        public string? Color { get; set; }
        public double Precio { get; set; }
    }
}
