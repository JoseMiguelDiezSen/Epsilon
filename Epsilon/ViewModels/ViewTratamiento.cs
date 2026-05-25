using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Persistencia.Modelos;

namespace Epsilon.ViewModels
{
    public class ViewTratamiento
    {
        public ViewTratamiento(DatosTratamientos datosTratamientos)
        {
            IdTratamiento = datosTratamientos.IdTratamiento;
            NomreTratamiento = datosTratamientos.NombreTratamiento;
            Duracion = datosTratamientos.Duracion;
            Color = datosTratamientos.Color;
            Precio = datosTratamientos.Precio;
        }

        public int IdTratamiento { get; set; }

        public string NomreTratamiento { get; set; }

        

        //public SelectList? NombreTratamiento { get; set; } = new SelectList(Enumerable.Empty<string>());

        public int Duracion { get; set; }

        public string? Color { get; set; }

        public double Precio { get; set; }
    }
}
