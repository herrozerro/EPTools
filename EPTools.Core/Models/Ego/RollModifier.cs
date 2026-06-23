using System.Text.Json.Serialization;

namespace EPTools.Core.Models.Ego;

public sealed class RollModifier
{
    public string Name { get; set; } = string.Empty;
    public RollModifierType Type { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; } = true;
    // Null = manually added by player. Non-null = name of the trait/ware that created this modifier.
    // Used by EgoManager to remove all modifiers from a source when that trait/ware is removed.
    public string? Source { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RollModifierType
{
    AptitudeCheck,
    Initiative,
    Skill,
    TraumaThreshold,
    InsanityRating,
    Lucidity,
    TraumasIgnored,
    Durability,
    DeathRating,
    WoundThreshold,
    WoundsIgnored,
    ArmorEnergy,
    ArmorKinetic,
    FlexPool,
    MoxiePool,
    VigorPool,
    InsightPool,
    TaskTimeFrame,
    Reputation,
    ResleeveIntegration,
    ResleeveTest,
    MovementRate,
    MovementRateFull
}