namespace Epsilon.Models
{
    /// <summary>
    /// Objeto DTO para las solicitudes enviadas al chat del asistente.
    /// </summary>
    public class BotChatRequest
    {
        /// <summary>
        /// Mensaje enviado por el usuario.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Contexto o ruta URL de la página actual.
        /// </summary>
        public string Context { get; set; } = string.Empty;
    }

    /// <summary>
    /// Objeto DTO para la respuesta devuelta por el controlador del chat.
    /// </summary>
    public class BotChatResponse
    {
        /// <summary>
        /// Texto de respuesta formateado para el cliente.
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }
}
