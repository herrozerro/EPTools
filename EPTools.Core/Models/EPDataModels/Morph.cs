using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Morph(
        string Book,
        string Name,
        string Type,
        int Cost,
        int Availability,
        int WoundThreshold,
        int Durability,
        int DeathRating,
        MorphPools Pools,
        List<MorphMovementRates> MovementRate,
        List<string> Ware,
        List<MorphTrait> MorphTraits,
        List<string> CommonExtras,
        List<string> Notes,
        List<string> CommonShapeAdjustments,
        string Image,
        string Description,
        string Resource,
        string Reference,
        string Id);

    public record MorphPools(
        int Insight,
        int Moxie,
        int Vigor,
        int Flex);

    public record MorphMovementRates(
        string MovementType,
        int Base,
        int Full);

    public record MorphTrait(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("level")] int Level);
}
