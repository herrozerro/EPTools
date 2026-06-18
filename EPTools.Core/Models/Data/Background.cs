namespace EPTools.Core.Models.Data;

public record Background(
    string Name,
    string Description,
    List<InterestSkill> Skills,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);