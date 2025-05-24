using System.ComponentModel.DataAnnotations;

namespace Epsilon.ViewModels
{
    public class ViewTratamiento
    {
        public int IdTratamiento { get; set; }

        public string? NombreTratamiento { get; set; }

        public int Precio { get; set; }
    }
}
