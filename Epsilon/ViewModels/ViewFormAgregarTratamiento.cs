using Microsoft.AspNetCore.Mvc.Rendering;

namespace Epsilon.ViewModels
{
    public class ViewFormAgregarTratamiento
    {
        public int IdTratamiento { get; set; }

        public SelectList? NombreTratamiento { get; set; }


    }
}
