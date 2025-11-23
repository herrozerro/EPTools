using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Interest(
        string Name,
        string Description,
        List<InterestSkill> Skills,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules
        );

    public record InterestSkill(
        string Name,
        int Rating,
        List<string> Options
        );
}
