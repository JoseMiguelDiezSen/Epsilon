using System.Net;

namespace Negocio.Servicios
{
    public class Informes
    {
        /// <summary>  
        /// Inicializa una nueva instancia de la clase <see cref="Informes"/>.  
        /// </summary>  
        /// <param name="context">El contexto de la base de datos.</param>  
        /// <param name="milogger">El logger.</param>  
        /// <param name="registroValidadores">El registro de validadores.</param>  
        /// <param name="seguridad">El servicio de seguridad.</param>  
        public Informes()
        {
        }

        /// <summary>  
        /// Genera un informe en el formato especificado.  
        /// </summary>  
        /// <param name="path">La ruta del informe.</param>  
        /// <param name="nombre">El nombre del informe.</param>  
        /// <param name="parametros">Los parámetros del informe.</param>  
        /// <param name="formato">El formato del informe (por defecto es "PDF").</param>  
        /// <returns>El informe generado en formato de bytes.</returns>  
        public byte[] GeneraInforme(string path, string nombre, Dictionary<string, string> parametros, string formato = "PDF")
        {
            var url = ConstruirUrl(path, nombre, parametros, formato);

            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = CredentialCache.DefaultCredentials;

                using (var client = new HttpClient(handler))
                {

                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsByteArrayAsync().Result;
                   
                }
            }
        }

        private string ConstruirUrl(string path, string nombre, Dictionary<string, string> parametros, string formato)
        {
            var baseUrl = "http://localhost/ReportServer?";


            //var baseUrl = "http://desktop-esknkl3:8080/ReportServer?";

            var reportPath = $"{path.TrimEnd('/')}/{nombre}";

            var parametrosUrl = string.Join("&", parametros.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));

            return $"{baseUrl}/{reportPath}&rs:Format={formato}&{parametrosUrl}";
        }
    }
}
    

