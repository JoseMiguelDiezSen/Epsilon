using Microsoft.AspNetCore.Mvc.Rendering;

namespace Epsilon.ViewModels
{
    public class ViewFormCorreoElectronico
    {
        public int IdCorreo { get; set; }

        public string? NombreCorreo { get; set; }

        public string? Asunto { get; set; }

        public string? CuerpoMensaje { get; set; }

        public string? NombreNuevoCorreo { get; set; }

        public SelectList ModelosCorreo { get; set; } = new SelectList(Enumerable.Empty<string>());
    }
}
