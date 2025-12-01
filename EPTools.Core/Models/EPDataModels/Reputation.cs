namespace EPTools.Core.Models.EPDataModels;

public record Reputation(
    string Name,
    string Description,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);