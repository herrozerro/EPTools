using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearVehicle(
        [property: JsonPropertyName("passengers")]int Passengers,
        [property: JsonPropertyName("vigor")]int Vigor,
        [property: JsonPropertyName("flex")]int Flex,
        [property: JsonPropertyName("armor_energy")]int ArmorEnergy,
        [property: JsonPropertyName("armor_kinetic")]int ArmorKinetic,
        [property: JsonPropertyName("wound_threshold")]int WoundThreshold,
        [property: JsonPropertyName("durability")]int Durability,
        [property: JsonPropertyName("death_rating")]int DeathRating,
        [property: JsonPropertyName("movement_rate")]MorphMovementRates MovementRate,
        [property: JsonPropertyName("size")]string Size,
        [property: JsonPropertyName("ware")]List<string> Ware,
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
