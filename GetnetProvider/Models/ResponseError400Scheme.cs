using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class ResponseError400Scheme
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("validation")]
        public ResponseError400SchemeValidation Validation { get; set; }
    }

    public class ResponseError400SchemeValidation
    {
        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("keys")]
        public IEnumerable<string> Keys { get; set; }
    }

}