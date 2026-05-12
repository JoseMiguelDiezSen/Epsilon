using Microsoft.AspNetCore.Http;

namespace Negocio.Servicios.Negocio.Servicios
{
    public interface IGestionEmail
    {
        bool EnviarEmailSinAdjunto(string to, string subject, string body);

        public bool EnviarEmailConAdjunto(string to, string subject, string body, IFormFile adjunto);

        public bool EnviarEmailConAdjuntoYReply(string emailTo, string subject, string body, string replyto, IFormFile adjunto);

    }
}