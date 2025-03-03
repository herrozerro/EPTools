using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Pool(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("effects")] List<string> Effects,
        [property: JsonPropertyName("checks")] List<string> Checks,
        [property: JsonPropertyName("resource")] string Resource,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("id")] string Id);
}
