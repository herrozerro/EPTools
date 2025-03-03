using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Morph(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("book")] string Book,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("cost")] int Cost,
        [property: JsonPropertyName("availability")] int Availability,
        [property: JsonPropertyName("wound_threshold")] int WoundThreshold,
        [property: JsonPropertyName("durability")] int Durability,
        [property: JsonPropertyName("death_rating")] int DeathRating,
        [property: JsonPropertyName("pools")] MorphPools Pools,
        [property: JsonPropertyName("movement_rate")] List<MorphMovementRates> MovementRate,
        [property: JsonPropertyName("ware")] List<string> Ware,
        [property: JsonPropertyName("morph_traits")] List<MorphTrait> MorphTraits,
        [property: JsonPropertyName("common_extras")] List<string> CommonExtras,
        [property: JsonPropertyName("notes")] List<string> Notes,
        [property: JsonPropertyName("common_shape_adjustments")] List<string> CommonShapeAdjustments,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("resource")] string Resource,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("id")] string Id
        );

    public record MorphPools(
        [property: JsonPropertyName("insight")] int Insight,
        [property: JsonPropertyName("moxie")] int Moxie,
        [property: JsonPropertyName("vigor")] int Vigor,
        [property: JsonPropertyName("flex")] int Flex);

    public record MorphMovementRates(
        [property: JsonPropertyName("movement_type")] string MovementType,
        [property: JsonPropertyName("base")] int Base,
        [property: JsonPropertyName("full")] int Full);

    public record MorphTrait(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("level")] int Level);
}
