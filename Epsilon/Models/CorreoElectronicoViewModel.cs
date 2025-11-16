using Microsoft.AspNetCore.Mvc.Rendering;

namespace Epsilon.Models
{
    public class CorreoElectronicoViewModel
    {
        public int IdCorreo { get; set; }

        public string? NombreCorreo { get; set; }

        public string? Asunto { get; set; }

        public string? CuerpoMensaje { get; set; }

        public string? NombreCorreoNuevo { get; set; }


    }
}
