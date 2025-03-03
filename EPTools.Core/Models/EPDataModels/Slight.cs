using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Slight(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("level")] string Level,
        [property: JsonPropertyName("duration")] string Duration,
        [property: JsonPropertyName("action")] string Action,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("resource")] string Resource,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("id")] string Id);
}
