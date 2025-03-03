using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearWeaponMelee(
        [property: JsonPropertyName("waretype")] string WareType,
        [property: JsonPropertyName("damage")] string Damage,
        [property: JsonPropertyName("damage_avg")] string DamageAvg,
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
    ) :
    Gear(Category, Subcategory, Name, Complexity, Description, Summary, Resource, Reference, Id);
}
