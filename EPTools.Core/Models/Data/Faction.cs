namespace EPTools.Core.Models.Data;

public record Faction(
    string Name,
    string Description,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);