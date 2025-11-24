using System.Text.Json.Serialization;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Models.EPDataModels
{
    public abstract record Gear(
        string Category,
        string Subcategory,
        string Name,
        string Complexity,
        string Description,
        string Summary,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules);

    public record AdditionalRules(
        string Name,
        string Description,
        AdditionalRuleType Type,
        int Value);
    
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
}