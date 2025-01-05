using System.Text.Json.Serialization;

namespace WordSearchLambda.Repository.Models
{
    public class License
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}