using System.Text.Json;

namespace Epsilon.Models.Comun
{
    public class JsonResponse
    {
        public JsonResponse(string status, string statusMsg, string data, string errorData)
        {
            Status = status;
            StatusMessage = statusMsg;
            Data = data;
            ErrorData = errorData;
        }

        public JsonResponse(string status, string statusMsg) : this(status, statusMsg, string.Empty, string.Empty) { }

        public JsonResponse(string status, string statusMsg, string data) : this(status, statusMsg, data, string.Empty) { }

        public string? Status { get; internal set; }

        public string? StatusMessage { get; internal set; }

        public string? Data { get; internal set; }

        public string? ErrorData { get; internal set; }

        public string Serializer() {
            return JsonSerializer.Serialize(this);
        }
    }
}
