namespace EPTools.Core.Models.Data;

public record Skill(
    string Name,
    string Aptitude,
    bool Active,
    bool Combat,
    bool Physical,
    bool Technical,
    bool Social,
    bool Know,
    bool Field,
    bool Mental,
    bool Psi,
    bool Vehicle,
    string Description,
    List<string> SampleFields,
    List<string> Specializations,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);