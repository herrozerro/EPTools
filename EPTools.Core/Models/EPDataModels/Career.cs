namespace EPTools.Core.Models.EPDataModels
{
    public record Career(
        string Name,
        string Description,
        List<InterestSkill> Skills,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules);
}
