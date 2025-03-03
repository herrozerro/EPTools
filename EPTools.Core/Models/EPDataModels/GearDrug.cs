using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearDrug(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("application")] string Application,
        [property: JsonPropertyName("duration")] string Duration,
        [property: JsonPropertyName("addiction")] string Addiction,
        string Category,
        string Subcategory,
        string Name,
        string Complexity,
        string Description,
        string Summary,
        string Resource,
        string Reference,
        string Id) : Gear(Category, Subcategory, Name, Complexity, Description, Summary, Resource, Reference, Id);
}
