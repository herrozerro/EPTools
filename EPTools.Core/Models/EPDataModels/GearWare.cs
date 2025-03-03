using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearWare(
        [property: JsonPropertyName("bioware")]bool Bioware,
        [property: JsonPropertyName("cyberware")]bool Cyberware,
        [property: JsonPropertyName("hardware")]bool Hardware,
        [property: JsonPropertyName("meshware")]bool Meshware,
        [property: JsonPropertyName("nanoware")]bool Nanoware,
        string Category,
        string Subcategory,
        string Name,
        string Complexity,
        string Description,
        string Summary,
        string Resource,
        string Reference,
        string Id
    ) : Gear(Category, Subcategory, Name, Complexity, Description, Summary, Resource, Reference, Id);
}
