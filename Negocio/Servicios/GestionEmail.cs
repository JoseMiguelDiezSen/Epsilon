using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Negocio.Persistencia;
using Negocio.Servicios.Comun;
using Negocio.Servicios.Negocio.Servicios;
using Negocio.Utilidades;
using Negocio.Validadores.Comun;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace Negocio.Servicios
{
    /// <summary>  
    /// Servicio de envio de correos electrónicos. 
    /// </summary>  
    public class GestionEmail : ServicioAbstractoEpsilon, IGestionEmail
    {
        /// <summary>
        /// Configuraciones de EMail
        /// </summary>
        protected EmailSettings _emailSettings;

        /// <summary>  
        /// Constructor de la clase 
        /// </summary>  
        /// <param name="context">El contexto de la base de datos.</param>  
        /// <param name="milogger">El logger.</param>  
        /// <param name="informes">El servicio de informes.</param>  
        public GestionEmail(EpsilonDbContext context, ILogger<GestionEmail> milogger, IValidadoresProgesfor registroValidadores, IOptions<EmailSettings> emailSettings) : base(context, milogger, registroValidadores)
        {
            _emailSettings = emailSettings.Value;
        }

        /// <summary>  
        /// Envía correo electrónico simple sin adjuntos 
        /// </summary>  
        public bool EnviarEmailSinAdjunto(string to, string subject, string body)
        {
            try
            {
                // From - To
                to = "jsm198969@gmail.com";
                var from = "jsm198969@gmail.com";

                // SMTP CLient
                string host = "smtp.gmail.com";
                int port = 587;

                // Network Credentials
                string userName = "jsm198969@gmail.com";
                string passwordApp = "prlrnmctuqrnpdgu";

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(from);
                    message.To.Add(new MailAddress(to));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(host, port))
                    {
                        smtpClient.Credentials = new NetworkCredential(userName, passwordApp);
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(message);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation(GetEventId(), ex.Message);
                throw;
            }
        }

        /// <summary>  
        /// Envía correo electrónico con informe adjunto
        /// </summary>  
        /// Este es el nuevo metodo que espera un archivo adjunto desde el frontend
        public bool EnviarEmailConAdjunto(string to, string subject, string body, IFormFile adjunto)
        {
            try
            {
                // From - To
                to = "jsm198969@gmail.com";
                var from = "jsm198969@gmail.com";

                // SMTP CLient
                string host = "smtp.gmail.com";
                int port = 587;

                // Network Credentials
                string userName = "jsm198969@gmail.com";
                string passwordApp = "prlrnmctuqrnpdgu";

                using (var message = new MailMessage(from, to))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    // Archivo Adjunto
                    if (adjunto != null && adjunto.Length > 0)
                    {
                        using (var stream = adjunto.OpenReadStream())
                        {
                            var attachment = new Attachment(stream, adjunto.FileName);
                            message.Attachments.Add(attachment);

                            using (var smtpClient = new SmtpClient(host, port))
                            {
                                smtpClient.Credentials = new NetworkCredential(userName, passwordApp);
                                smtpClient.EnableSsl = true;
                                smtpClient.Send(message);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation(GetEventId(), ex.Message);
                throw;
            }
        }

        /// <summary>  
        /// Envía correo electrónico con informe adjunto y respuesta  
        /// </summary>  
        /// <param name="emailTo">El correo electrónico del destinatario.</param>  
        public bool EnviarEmailConAdjuntoYReply(string emailTo,string subject,string body,string replyto, IFormFile file)
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + ": " + emailTo);

            var memoriaAdjuntos = new List<MemoryStream>();

            try
            {
                // From - To
                emailTo = "jsm198969@gmail.com";
                var from = "jsm198969@gmail.com";

                // SMTP CLient
                string host = "smtp.gmail.com";
                int port = 587;

                // Network Credentials
                string userName = "jsm198969@gmail.com";
                string passwordApp = "prlrnmctuqrnpdgu";

                using (var message = new MailMessage(from, emailTo))
                {
                    message.Subject = subject;
                    message.Body = body;

                    if (!string.IsNullOrWhiteSpace(replyto))
                        message.ReplyToList.Add(new MailAddress(replyto));

                    if (file != null && file.Length > 0)
                    {
                        var memoryStream = new MemoryStream();
                        file.CopyTo(memoryStream);
                        memoryStream.Position = 0;

                        memoriaAdjuntos.Add(memoryStream);

                        var attachment = new Attachment(memoryStream, file.FileName);
                        message.Attachments.Add(attachment);
                    }

                    using (var smtpClient = new SmtpClient(host, port))
                    {
                        smtpClient.Credentials = new NetworkCredential(userName, passwordApp);
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(message);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation(GetEventId(), ex.Message);
                throw;
            }
            finally
            {
                foreach (var ms in memoriaAdjuntos)
                    ms.Dispose();
            }
        }
    }
}
