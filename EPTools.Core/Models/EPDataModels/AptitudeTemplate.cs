using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record AptitudeTemplate(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")]string Description,
        [property: JsonPropertyName("aptitudes")]AptitudeTemplateAptitudes Aptitudes,
        [property: JsonPropertyName("resource")]string Resource,
        [property: JsonPropertyName("reference")]string Reference,
        [property: JsonPropertyName("id")]string Id);
    
    public record AptitudeTemplateAptitudes(
        [property: JsonPropertyName("cognition")]int Cognition,
        [property: JsonPropertyName("intuition")]int Intuition,
        [property: JsonPropertyName("reflexes")]int Reflexes,
        [property: JsonPropertyName("savvy")]int Savvy,
        [property: JsonPropertyName("somatics")]int Somatics,
        [property: JsonPropertyName("willpower")]int Willpower);
}
