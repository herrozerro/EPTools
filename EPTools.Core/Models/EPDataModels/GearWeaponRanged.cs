using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearWeaponRanged(
        string WareType,
        string Damage,
        string DamageAvg,
        string FireModes,
        int Ammo,
        int Range,
        string Notes,
        string Category,
        string Subcategory,
        string Name,
        string Complexity,
        string Description,
        string Summary,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules
    ) : Gear(Category, Subcategory, Name, Complexity, Description, Summary, Resource, Reference, AdditionalRules);
}
