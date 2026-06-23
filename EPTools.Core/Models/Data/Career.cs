namespace EPTools.Core.Models.Data;

public record Career(
    string Name,
    string Description,
    List<InterestSkill> Skills,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);