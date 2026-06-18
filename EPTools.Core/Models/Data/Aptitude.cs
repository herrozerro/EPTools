namespace EPTools.Core.Models.Data;

public record Aptitude(
    string Name,
    string Description,
    string ShortName,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);