using Epsilon.Models;
using Epsilon.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epsilon.Controllers
{
    /// <summary>
    /// Controlador API para gestionar las interacciones con el asistente de IA flotante.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly ILogger<BotController> _logger;

        public BotController(IGeminiService geminiService, ILogger<BotController> logger)
        {
            _geminiService = geminiService;
            _logger = logger;
        }

        /// <summary>
        /// Recibe las preguntas enviadas desde la interfaz web del chat y devuelve la respuesta del servicio de IA.
        /// </summary>
        /// <param name="request">Objeto con el mensaje enviado y la ubicación o contexto de la página.</param>
        /// <returns>Objeto JSON con la respuesta o código HTTP 503 si el servicio no responde.</returns>
        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] BotChatRequest request)
        {
            // Validar que la petición sea válida y contenga texto
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("El mensaje del usuario no puede estar vacío.");
            }

            // Invocar el servicio de Gemini con contexto de la base de datos
            var respuesta = await _geminiService.GetChatResponseAsync(request.Message, request.Context);

            // Si la IA no está disponible o falla, devolver HTTP 503 para que el cliente muestre el mensaje de contingencia
            if (string.IsNullOrEmpty(respuesta))
            {
                _logger.LogWarning("[BotController] El servicio de IA no devolvió respuesta. Devolviendo estado 503.");
                return StatusCode(503, new { error = "AI_UNAVAILABLE" });
            }

            return Ok(new BotChatResponse
            {
                Text = respuesta
            });
        }
    }
}
