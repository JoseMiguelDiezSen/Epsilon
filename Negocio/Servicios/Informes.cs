using Microsoft.Extensions.Logging;
using Negocio.Persistencia;
using Negocio.Servicios.Comun;
using Negocio.Validadores.Comun;
using System.Net;

namespace Negocio.Servicios
{
    public class Informes: ServicioAbstractoEpsilon, IInformes
    {
        private readonly EpsilonDbContext _context;
        private readonly string _reportServerBase = "http://localhost:8080/ReportServer";

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
        /// Genera un informe de forma sincrónica delegando en la versión asíncrona.
        /// </summary>
        /// <param name="path">La ruta del informe.</param>
        /// <param name="nombre">El nombre del informe.</param>
        /// <param name="parametros">Los parámetros del informe.</param>
        /// <param name="formato">El formato del informe (por defecto es "PDF").</param>
        /// <returns>El informe generado en formato de bytes.</returns>
        public byte[] GeneraInforme(string path, string nombre, Dictionary<string,string> parametros, string formato = "PDF")
        {
            return GeneraInformeAsync(path, nombre, parametros, formato).GetAwaiter().GetResult();
        }

        private string ConstruirUrl(string path, string nombre, Dictionary<string, string> parametros, string formato)
        {
            var reportPath = $"{path.Trim('/')}/{nombre}";
            var parametrosUrl = string.Join("&", parametros.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            // Formato correcto: http://host/ReportServer?/Folder/Report&rs:Format=PDF&param=1
            return $"{_reportServerBase}?/{reportPath}&rs:Format={formato}&{parametrosUrl}";
        }

        //Desde la opcion la opcion Extensiones --> Administrar Extensiones se ha instalado el paquete Reporting Services Projects

    }
}

