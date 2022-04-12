using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class CreateCardTokenResponse
    {
        [JsonPropertyName("number_token")]
        public string NumberToken { get; set; }
    }
}