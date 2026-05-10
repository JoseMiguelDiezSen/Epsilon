namespace Negocio.Servicios.Negocio.Servicios
{
    public interface IGestionEmail
    {
        bool EnviarEmailSinAdjunto(string to, string subject, string body);

        bool EnviarEmailConAdjunto(
            string reportPath,
            string reportName,
            Dictionary<string, string> parameters,
            string format,
            string emailTo,
            string subject,
            string body);

        bool EnviarEmailConAdjuntoYReply(
            string emailTo,
            string subject,
            string body,
            string replyto,
            List<(byte[] Data, string FileName)> documents);
    }
}