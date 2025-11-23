using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Background(
            string Name,
            string Description,
            List<InterestSkill> Skills,
            string Resource,
            string Reference,
            List<AdditionalRules> AdditionalRules);
}
