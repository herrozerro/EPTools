namespace EPTools.Core.Models.Data;

public record Pool(
    string Name,
    List<string> Effects,
    List<string> Checks,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);