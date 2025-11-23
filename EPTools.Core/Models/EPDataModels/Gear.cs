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
        AdditionalRuleType Type,
        int Value);
    
    public enum AdditionalRuleType {
        Skill,
        Attribute,
        Gear
    }
}