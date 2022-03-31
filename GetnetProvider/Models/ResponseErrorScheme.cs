using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class ResponseErrorScheme<T> where T : class
    {
        [JsonPropertyName("message")]
        [StringLength(250)]
        public string Message { get; set; }

        [StringLength(250)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status_code")]
        public dynamic StatusCode { get; set; }

        [JsonPropertyName("details")]
        public IEnumerable<T> Details { get; set; } = Array.Empty<T>();
    }

    public class DetailsRequestErrorScheme
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("error_code")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("description_detail")]
        public string DescriptionDetail { get; set; }
    }
}