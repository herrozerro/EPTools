using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Aptitude(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")]string Description,
        [property: JsonPropertyName("short_name")]string ShortName,
        [property: JsonPropertyName("resource")]string Resource,
        [property: JsonPropertyName("reference")]string Reference,
        [property: JsonPropertyName("id")]string Id);
}
