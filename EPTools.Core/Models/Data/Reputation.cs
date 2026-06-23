namespace EPTools.Core.Models.Data;

public record Reputation(
    string Name,
    string Description,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);