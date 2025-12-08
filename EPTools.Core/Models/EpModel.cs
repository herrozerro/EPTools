using System.Text.Json.Serialization;
using EPTools.Core.Models.EPDataModels;

namespace EPTools.Core.Models;

public abstract class EpModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<AdditionalRules> AdditionalRules { get; set; } = [];
}

public class AdditionalRules
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AdditionalRuleType RuleType { get; set; }
    public int Value { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AdditionalRuleType {
    SkillBonus,
    SkillAdded,
    AptitudeCheck,
    Pool,
    Recharge,
    Armor,
    Weapon,
    WoundThreshold,
    Durability,
    TraumaThreshold,
    Lucidity,
    InsanityRating,
    IgnoreWound,
    IgnoreTrauma,
    DamageValueAdded,
    MovementRate,
    MovementRateFull
}