using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Epsilon.ViewModels
{
    public class ViewTratamiento
    {
        public int IdTratamiento { get; set; }

        public SelectList? NombreTratamiento { get; set; } = new SelectList(Enumerable.Empty<string>());

        public int Precio { get; set; }
    }
}
