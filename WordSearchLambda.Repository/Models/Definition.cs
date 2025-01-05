using System.Text.Json.Serialization;

namespace WordSearchLambda.Repository.Models
{
    public class Definition
    {
        [JsonPropertyName("definition")]
        public string DefinitionText { get; set; }

        [JsonPropertyName("example")]
        public string Example { get; set; }
    }
}