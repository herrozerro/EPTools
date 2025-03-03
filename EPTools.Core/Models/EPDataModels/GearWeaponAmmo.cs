using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearWeaponAmmo(
        [property: JsonPropertyName("damage")] string Damage,
        [property: JsonPropertyName("notes")] string Notes,
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
