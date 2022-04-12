using System.Text.Json.Serialization;

namespace GetnetProvider.Models
{
    public class GetIPDeviceResponse    
    {
        [JsonPropertyName("ip")]
        public string IP { get; set; }
    }   
}