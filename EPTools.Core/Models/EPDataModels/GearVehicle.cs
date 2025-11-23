using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearVehicle(
        int Passengers,
        int Vigor,
        int Flex,
        int ArmorEnergy,
        int ArmorKinetic,
        int WoundThreshold,
        int Durability,
        int DeathRating,
        MorphMovementRates MovementRate,
        string Size,
        List<string> Ware,
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
