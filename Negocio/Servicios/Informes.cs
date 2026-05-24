using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Servicios.Comun;
using Negocio.Utilidades;
using Negocio.Validadores.Comun;
using System.Net;

namespace Negocio.Servicios
{
    public class Informes: ServicioAbstractoEpsilon, IInformes
    {
        private readonly EpsilonDbContext _context;
        private readonly string _reportServerBase = "http://localhost:8080/ReportServer";

        /// <summary>
        /// Configuraciones de SSRS
        /// </summary>
        protected SSRSSettings _SSRSSettings;
     
        /// <summary>  
        /// Inicializa una nueva instancia de la clase <see cref="Informes"/>.  
        /// </summary>  
        /// <param name="context">El contexto de la base de datos.</param>  
        /// <param name="milogger">El logger.</param>  
        /// <param name="registroValidadores">El registro de validadores.</param>  
        /// <param name="seguridad">El servicio de seguridad.</param>  
        public Informes(EpsilonDbContext context, ILogger<GestionUsuarios> logger, IValidadoresProgesfor registroValidadores) : base(context, logger, registroValidadores)
        { 
            _context = context;
        }

        /// <summary>
        /// Genera un informe de forma sincrónica delegando en la versión asíncrona.
        /// </summary>
        /// <param name="path">La ruta del informe.</param>
        /// <param name="nombre">El nombre del informe.</param>
        /// <param name="parametros">Los parámetros del informe.</param>
        /// <param name="formato">El formato del informe (por defecto es "PDF").</param>
        /// <returns>El informe generado en formato de bytes.</returns>
        public byte[] GeneraInforme(string path, string nombre, Dictionary<string, string> parametros, string formato = "PDF")
        {
            return GeneraInformeAsync(path, nombre, parametros, formato).GetAwaiter().GetResult();
        }

        /// <summary>  
        /// Genera un informe en el formato especificado.  
        /// </summary>  
        /// <param name="path">La ruta del informe.</param>  
        /// <param name="nombre">El nombre del informe.</param>  
        /// <param name="parametros">Los parámetros del informe.</param>  
        /// <param name="formato">El formato del informe (por defecto es "PDF").</param>  
        /// <returns>El informe generado en formato de bytes.</returns>  
        public async Task<byte[]> GeneraInformeAsync(string path, string nombre, Dictionary<string,string> parametros, string formato = "PDF")
        {
            var url = ConstruirUrl(path, nombre, parametros, formato);

            using (var handler = new HttpClientHandler { Credentials = CredentialCache.DefaultCredentials })
            using (var client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Construye la URL necesaria para solicitar un informe a SSRS vía HTTP.
        /// </summary>
        /// <param name="path">Ruta del informe dentro del servidor (ej: /CarpetaInformes)</param>
        /// <param name="nombre">Nombre del informe (ej: InformePacientes)</param>
        /// <param name="parametros">Parámetros que requiere el informe (clave/valor)</param>
        /// <param name="formato">Formato de salida (PDF, Excel, etc.)</param>
        /// <returns>URL completa lista para consumir el informe</returns>
        private string ConstruirUrl(string path, string nombre, Dictionary<string, string> parametros, string formato)
        {
            // Limpia posibles "/" al inicio o final del path y construye la ruta completa del informe
            // Ejemplo: "/Carpeta/" + "Informe" → "Carpeta/Informe"
            var reportPath = $"{path.Trim('/')}/{nombre}";

            // Convierte el diccionario de parámetros en formato URL:
            // { IdPaciente: 5, Tipo: "A" } → "IdPaciente=5&Tipo=A"
            // EscapeDataString evita problemas con espacios, acentos, etc.
            var parametrosUrl = string.Join("&", parametros.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));

            // Formato esperado por SSRS:
            // http://servidor/ReportServer?/Carpeta/Informe&rs:Format=PDF&param1=valor1
            // - ?/ indica ruta del informe dentro de SSRS
            // - rs:Format indica el formato de salida
            // - después van los parámetros

            return $"{_reportServerBase}?/{reportPath}&rs:Format={formato}&{parametrosUrl}";
        }

        /// <summary>  
        /// Renderiza un informe de manera asíncrona.  
        /// </summary>  
        /// <param name="reportPath">La ruta del informe.</param>  
        /// <param name="reportName">El nombre del informe.</param>  
        /// <param name="parameters">Los parámetros del informe.</param>  
        /// <param name="formatType">El tipo de formato del informe (por defecto es "PDF").</param>  
        /// <returns>La respuesta de renderizado del informe.</returns>  
        public async Task<byte[]> RenderReport(string reportPath, string reportName, Dictionary<string, string> parameters, string formatType = "PDF")
        {
            try
            {
                var url = ConstruirUrl(reportPath, reportName, parameters, formatType);

                using var handler = new HttpClientHandler
                {
                    Credentials = new NetworkCredential(
                        _SSRSSettings.User,
                        _SSRSSettings.Password,
                        _SSRSSettings.Domain
                    )
                };

                using var client = new HttpClient(handler);

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}

