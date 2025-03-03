using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearCreature(
        [property: JsonPropertyName("attributes")] Attributes Attributes,
        [property: JsonPropertyName("movement_rate")] MorphMovementRates MovementRate,
        [property: JsonPropertyName("ware")] List<string> Ware,
        [property: JsonPropertyName("skills")] List<string> Skills,
        [property: JsonPropertyName("traits")] MorphTrait Traits,
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

    public record Attributes(
        [property: JsonPropertyName("cognition")] int Cognition,
        [property: JsonPropertyName("cognition_check")] int CognitionCheck,
        [property: JsonPropertyName("intuition")] int Intuition,
        [property: JsonPropertyName("intuition_check")] int IntuitionCheck,
        [property: JsonPropertyName("reflexes")] int Reflexes,
        [property: JsonPropertyName("reflexes_check")] int ReflexesCheck,
        [property: JsonPropertyName("savvy")] int Savvy,
        [property: JsonPropertyName("savvy_check")] int SavvyCheck,
        [property: JsonPropertyName("somatics")] int Somatics,
        [property: JsonPropertyName("somatics_check")] int SomaticsCheck,
        [property: JsonPropertyName("willpower")] int Willpower,
        [property: JsonPropertyName("willpower_check")] int WillpowerCheck,
        [property: JsonPropertyName("initiative")] int Initiative,
        [property: JsonPropertyName("tp")] int Tp,
        [property: JsonPropertyName("armor_energy")] int ArmorEnergy,
        [property: JsonPropertyName("armor_kinetic")] int ArmorKinetic,
        [property: JsonPropertyName("wound_threshold")] int WoundThreshold,
        [property: JsonPropertyName("durability")] int Durability,
        [property: JsonPropertyName("death_rating")] int DeathRating,
        [property: JsonPropertyName("trauma_threshold")] int TraumaThreshold,
        [property: JsonPropertyName("lucidity")] int Lucidity,
        [property: JsonPropertyName("insanity_rating")] int InsanityRating);
}
