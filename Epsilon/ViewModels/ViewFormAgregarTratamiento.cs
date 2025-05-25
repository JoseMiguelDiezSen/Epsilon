using Microsoft.AspNetCore.Mvc.Rendering;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarTratamiento
    {
        public int IdTratamiento { get; set; }

        //public string? NombreTratamiento { get; set; }

        public SelectList? NombreTratamiento { get; set; }

        public int Precio { get; set; }
    }
}
