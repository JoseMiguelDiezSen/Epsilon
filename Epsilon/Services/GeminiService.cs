using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Negocio.Persistencia;

namespace Epsilon.Services
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de integración con la IA Gemini de Google.
    /// </summary>
    public interface IGeminiService
    {
        /// <summary>
        /// Procesa la consulta del usuario enviando el contexto real de la base de datos a la API de Gemini.
        /// </summary>
        /// <param name="userMessage">Mensaje enviado por el usuario.</param>
        /// <param name="context">Ruta o sección de la aplicación desde la que se realiza la consulta.</param>
        /// <returns>Respuesta generada por la IA o null si el servicio no está disponible.</returns>
        Task<string?> GetChatResponseAsync(string userMessage, string context);
    }

    /// <summary>
    /// Implementación del servicio de IA Gemini para Epsilon.
    /// Consulta datos reales de SQL Server y los inyecta en el prompt para ofrecer respuestas precisas.
    /// </summary>
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeminiService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public GeminiService(
            HttpClient httpClient, 
            IConfiguration configuration, 
            ILogger<GeminiService> logger,
            IServiceProvider serviceProvider)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<string?> GetChatResponseAsync(string userMessage, string context)
        {
            // 1. Obtener la API Key configurada en appsettings.json
            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("[GeminiService] Falta la API Key de Gemini en el archivo appsettings.json.");
                return null;
            }

            // 2. Extraer información en tiempo real desde la base de datos SQL Server
            string contextoDatosBBDD = await ConsultarDatosSistemaAsync();

            // 3. Construir las instrucciones del sistema para la IA
            string promptSistema = $@"Eres Epsilon AI, el asistente virtual corporativo de la plataforma 'Epsilon - Gestión Empresarial'.
Tu objetivo es responder de forma profesional, precisa y clara a las consultas sobre la operativa de la empresa.

INFORMACIÓN EN TIEMPO REAL DE LA BASE DE DATOS DE EPSILON:
{contextoDatosBBDD}

INSTRUCCIONES DE RESPUESTA:
- Responde siempre basándote en los datos reales proporcionados arriba.
- Sé conciso, educado y ejecutivo en tus respuestas.
- Utiliza negrita para destacar datos o cifras importantes.";

            // 4. Estructura del cuerpo de la petición según la especificación de Google Gemini API
            var peticionJson = new
            {
                system_instruction = new { parts = new[] { new { text = promptSistema } } },
                contents = new[] { new { parts = new[] { new { text = string.IsNullOrWhiteSpace(context) ? userMessage : $"[Ubicación actual: {context}]\n{userMessage}" } } } }
            };

            // 5. Modelos disponibles de Google Gemini en orden de precedencia (Sistema en cascada / Waterfall)
            string[] modelosCascada = { "gemini-3.8-flash", "gemini-3.7-flash", "gemini-3.5-flash", "gemini-flash-latest" };

            // 6. Recorrer la lista de modelos hasta obtener una respuesta correcta
            foreach (var modelo in modelosCascada)
            {
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent?key={apiKey}";

                try
                {
                    var response = await _httpClient.PostAsJsonAsync(url, peticionJson);

                    if (response.IsSuccessStatusCode)
                    {
                        var resultadoJson = await response.Content.ReadFromJsonAsync<JsonElement>();
                        if (resultadoJson.TryGetProperty("candidates", out var candidatos) &&
                            candidatos.GetArrayLength() > 0 &&
                            candidatos[0].TryGetProperty("content", out var contenido) &&
                            contenido.TryGetProperty("parts", out var partes) &&
                            partes.GetArrayLength() > 0 &&
                            partes[0].TryGetProperty("text", out var elementoTexto))
                        {
                            return elementoTexto.GetString();
                        }
                    }
                    else
                    {
                        var detalleError = await response.Content.ReadAsStringAsync();
                        _logger.LogWarning("[GeminiService] El modelo {Modelo} devolvió estado {Codigo}. Reintentando siguiente modelo... Detalle: {Error}", modelo, response.StatusCode, detalleError);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "[GeminiService] Excepción al conectar con el modelo {Modelo}: {Mensaje}", modelo, ex.Message);
                }
            }

            _logger.LogError("[GeminiService] Todos los modelos de la API de Gemini han fallado.");
            return null;
        }

        /// <summary>
        /// Consulta la base de datos SQL Server para compilar un resumen con las métricas actuales del sistema.
        /// </summary>
        private async Task<string> ConsultarDatosSistemaAsync()
        {
            var sb = new StringBuilder();

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<EpsilonDbContext>();

                // Consultar facturas registradas
                var facturas = await db.Facturacion.AsNoTracking().ToListAsync();
                if (facturas.Any())
                {
                    sb.AppendLine($"[FACTURACIÓN]");
                    sb.AppendLine($"- Total facturado acumulado: {facturas.Sum(f => f.Importe):N2} € (Total de {facturas.Count} facturas).");

                    var facturasPorMes = facturas
                        .Where(f => f.FechaFactura.Year > 2000)
                        .GroupBy(f => new { f.FechaFactura.Year, f.FechaFactura.Month })
                        .OrderByDescending(g => g.Key.Year).ThenByDescending(g => g.Key.Month)
                        .ToList();

                    var ci = new System.Globalization.CultureInfo("es-ES");
                    foreach (var grupo in facturasPorMes)
                    {
                        string nombreMes = ci.DateTimeFormat.GetMonthName(grupo.Key.Month);
                        sb.AppendLine($"  * {nombreMes.ToUpper()} {grupo.Key.Year}: {grupo.Sum(f => f.Importe):N2} € ({grupo.Count()} facturas).");
                    }
                }
                else
                {
                    sb.AppendLine("[FACTURACIÓN]: No existen registros de facturación acumulados en el sistema.");
                }

                // Consultar métricas de citas
                var totalCitas = await db.Citas.AsNoTracking().CountAsync();
                var citasHoy = await db.Citas.AsNoTracking().CountAsync(c => c.FechaInicio.Date == DateTime.Today);
                sb.AppendLine($"\n[CITAS Y AGENDA]");
                sb.AppendLine($"- Total de citas en el sistema: {totalCitas}.");
                sb.AppendLine($"- Citas programadas para hoy ({DateTime.Today:dd/MM/yyyy}): {citasHoy}.");

                // Consultar totales de clientes
                var totalClientes = await db.Clientes.AsNoTracking().CountAsync();
                sb.AppendLine($"\n[CLIENTES / PACIENTES]");
                sb.AppendLine($"- Total de clientes registrados: {totalClientes}.");

                // Consultar totales de usuarios
                var totalUsuarios = await db.Usuarios.AsNoTracking().CountAsync();
                sb.AppendLine($"\n[USUARIOS Y PERSONAL]");
                sb.AppendLine($"- Total de usuarios del sistema: {totalUsuarios}.");

                var totalPersonal = await db.Personal.AsNoTracking().CountAsync();
                sb.AppendLine($"- Total de personal/especialistas: {totalPersonal}.");

                // Consultar catálogo de servicios
                var servicios = await db.Servicios.AsNoTracking().Take(10).ToListAsync();
                if (servicios.Any())
                {
                    sb.AppendLine("\n[CATÁLOGO DE SERVICIOS]");
                    foreach (var s in servicios)
                    {
                        sb.AppendLine($"  * {s.NombreServicio}: {s.Precio:N2} € (Duración: {s.Duracion} min).");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[GeminiService] Error al consultar la base de datos SQL Server.");
            }

            return sb.ToString();
        }
    }
}
