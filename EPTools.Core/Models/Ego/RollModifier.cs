using System.Text.Json.Serialization;

namespace EPTools.Core.Models.Ego;

public sealed class RollModifier
{
    public string Name { get; set; } = string.Empty;
    public RollModifierType Type { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDerived { get; set; } = false;
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