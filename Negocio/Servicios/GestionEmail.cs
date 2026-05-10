using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

//using System.IO;
//using System.Reflection;
//using System.Security.Principal;
//using System.ServiceModel;
//using System.ServiceModel.Security;
//using System.Xml.Linq;

using Negocio.Persistencia;
using Negocio.Servicios.Comun;
using Negocio.Servicios.Negocio.Servicios;
//using Microsoft.IdentityModel.Protocols;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using Negocio.Utilidades;
//using System.Reflection;
//using Negocio.Persistencia.Modelos;
//using Negocio.Servicios.Comun;
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
        /// Interfaz de informes para generar los adjuntos de los correos electrónicos
        /// </summary>
        private readonly IInformes? _informes;

        /// <summary>
        /// Configuraciones de SSRS
        /// </summary>
        protected SSRSSettings _SSRSSettings;

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
        public GestionEmail(EpsilonDbContext context, ILogger<GestionEmail> milogger, IValidadoresProgesfor registroValidadores, IInformes informes, IOptions<SSRSSettings> ssrsSettings, IOptions<EmailSettings> emailSettings) : base(context, milogger, registroValidadores)
        {
            _informes = informes;
            _SSRSSettings = ssrsSettings.Value;
            _emailSettings = emailSettings.Value;
        }

        /// <summary>  
        /// Envía correo electrónico simple sin adjuntos 
        /// </summary>  
        public bool EnviarEmailSinAdjunto(string to, string subject, string body)
        {
            try
            {
                var from = _emailSettings.User;
                var password = _emailSettings.Password;

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(from);
                    message.To.Add(new MailAddress(to));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(_emailSettings.Server, int.Parse(_emailSettings.Port)))
                    {
                        smtpClient.Credentials = new NetworkCredential(from, password);
                        smtpClient.EnableSsl = false;

                        smtpClient.Send(message);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>  
        /// Envía correo electrónico con informe adjunto
        /// </summary>  
        public bool EnviarEmailConAdjunto(string reportPath, string reportName, Dictionary<string, string> parameters, string format, string emailTo, string subject, string body)
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + "(" + reportPath + "/" + reportName + ")");
            try
            {
                var result = _informes.RenderReport(reportPath, reportName, parameters, format).GetAwaiter().GetResult();

                emailTo = "jsm.198969@gmail.com"; // solo para pruebas

                using (var memoryStream = new MemoryStream(result))
                {
                    using (var message = new MailMessage("no_responder@gmail.com", emailTo))
                    {
                        message.Subject = subject;
                        message.Body = body;
                        message.Attachments.Add(new Attachment(memoryStream, $"Reporte.{format.ToLower()}"));

                        using (var smtpClient = new SmtpClient(_emailSettings.Server, int.Parse(_emailSettings.Port)))
                        {
                            smtpClient.Credentials = new NetworkCredential(_emailSettings.User, _emailSettings.Password);
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.EnableSsl = false;
                            smtpClient.SendMailAsync(message).Wait();
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
        public bool EnviarEmailConAdjuntoYReply(string emailTo,string subject,string body,string replyto,List<(byte[] Data, string FileName)> documents)
        {
            logger.LogTrace(GetEventId(), MethodBase.GetCurrentMethod()?.Name + ": " + emailTo);

            var memoriaAdjuntos = new List<MemoryStream>();

            try
            {
                emailTo = "jsm.198969@gmail.com"; // solo para pruebas

                using (var message = new MailMessage(_emailSettings.User, emailTo))
                {
                    message.Subject = subject;
                    message.Body = body;

                    if (!string.IsNullOrWhiteSpace(replyto))
                        message.ReplyToList.Add(new MailAddress(replyto));

                    if (documents != null)
                    {
                        foreach (var doc in documents)
                        {
                            if (doc.Data == null) continue;

                            var memoryStream = new MemoryStream(doc.Data);
                            memoriaAdjuntos.Add(memoryStream);

                            var attachment = new Attachment(memoryStream, doc.FileName, "application/pdf");
                            message.Attachments.Add(attachment);
                        }
                    }
                    using (var smtpClient = new SmtpClient(_emailSettings.Server, int.Parse(_emailSettings.Port)))
                    {
                        smtpClient.Credentials = new NetworkCredential(_emailSettings.User, _emailSettings.Password);
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.EnableSsl = false;

                        smtpClient.SendMailAsync(message).Wait();
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
