using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Skill(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("aptitude")] string Aptitude,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("combat")] bool Combat,
        [property: JsonPropertyName("physical")] bool Physical,
        [property: JsonPropertyName("technical")] bool Technical,
        [property: JsonPropertyName("social")] bool Social,
        [property: JsonPropertyName("know")] bool Know,
        [property: JsonPropertyName("field")] bool Field,
        [property: JsonPropertyName("mental")] bool Mental,
        [property: JsonPropertyName("psi")] bool Psi,
        [property: JsonPropertyName("vehicle")] bool Vehicle,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("sample_fields")] List<string> SampleFields,
        [property: JsonPropertyName("specializations")] List<string> Specializations,
        [property: JsonPropertyName("resource")] string Resource,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("id")] string Id);
}
