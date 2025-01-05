using System.Text.Json.Serialization;

namespace WordSearchLambda.Repository.Models
{
    public class Phonetic
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } 

        [JsonPropertyName("audio")]
        public string Audio { get; set; } 

        [JsonPropertyName("sourceUrl")]
        public string SourceUrl { get; set; } 

        [JsonPropertyName("license")]
        public License License { get; set; } 
    }
}