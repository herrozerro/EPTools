using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearCategories(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("text")] string Text,
        [property: JsonPropertyName("subcategories")] GearSubcategories Subcategories,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("resource")] string Resource);

    public record GearSubcategories(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("text")] string Text);
}
