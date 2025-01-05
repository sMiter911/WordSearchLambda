using System.Text.Json.Serialization;

namespace WordSearchLambda.Repository.Models
{
    public class Meaning
    {
        [JsonPropertyName("partOfSpeech")]
        public string PartOfSpeech { get; set; }

        [JsonPropertyName("definitions")]
        public List<Definition> Definitions { get; set; }
    }
}