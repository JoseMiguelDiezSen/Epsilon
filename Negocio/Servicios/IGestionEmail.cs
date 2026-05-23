using Microsoft.AspNetCore.Http;

namespace Negocio.Servicios.Negocio.Servicios
{
    public interface IGestionEmail
    {
        /// <summary>
        /// Sends an email message without attachments to the specified recipient.
        /// </summary>
        /// <param name="to">The email address of the recipient. Cannot be null or empty.</param>
        /// <param name="subject">The subject line of the email message. Cannot be null.</param>
        /// <param name="body">The body content of the email message. Cannot be null.</param>
        /// <returns>true if the email was sent successfully; otherwise, false.</returns>
        bool EnviarEmailSinAdjunto(string to, string subject, string body);

        /// <summary>
        /// Envía un correo electrónico con un archivo adjunto al destinatario especificado.
        /// </summary>
        /// <param name="to">La dirección de correo electrónico del destinatario. No puede ser null ni estar vacía.</param>
        /// <param name="subject">El asunto del correo electrónico. Puede estar vacío si no se requiere un asunto.</param>
        /// <param name="body">El contenido del mensaje del correo electrónico. Puede estar vacío.</param>
        /// <param name="adjunto">El archivo que se adjuntará al correo electrónico. No puede ser null.</param>
        /// <returns>true si el correo electrónico se envía correctamente; en caso contrario, false.</returns>
        public bool EnviarEmailConAdjunto(string to, string subject, string body, IFormFile adjunto);

        /// <summary>
        /// Sends an email message with an attachment and specifies a reply-to address.
        /// </summary>
        /// <param name="emailTo">The recipient's email address. Cannot be null or empty.</param>
        /// <param name="subject">The subject line of the email message.</param>
        /// <param name="body">The body content of the email message.</param>
        /// <param name="replyto">The email address to be used as the reply-to address. Cannot be null or empty.</param>
        /// <param name="adjunto">The file to attach to the email message. Must be a valid file or null if no attachment is required.</param>
        /// <returns>true if the email was sent successfully; otherwise, false.</returns>
        public bool EnviarEmailConAdjuntoYReply(string emailTo, string subject, string body, string replyto, IFormFile adjunto);
    }
}