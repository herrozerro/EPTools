using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Trait(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("cost")] List<int> Cost,
        [property: JsonPropertyName("ego")] bool Ego,
        [property: JsonPropertyName("morph")] bool Morph,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("auto")] bool Auto,
        [property: JsonPropertyName("noted")] bool Noted,
        [property: JsonPropertyName("resource")] string Resource,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("id")] string Id
        );
}
