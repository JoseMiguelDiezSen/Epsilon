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



        public string? NombrePaciente { get; set; }
        public string? EmailPaciente { get; set; }



        public IFormFile InputAdjuntos { get; set; } // Coincide con name="inputAdjuntos"

        public bool ConAdjuntos { get; set; } // Para el checkbox chkAdjuntos
        public bool SolicitarRespuesta { get; set; } // Para el che
    }
}
