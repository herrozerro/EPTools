namespace EPTools.Core.Models.Data;

public record AptitudeTemplate(
    string Name,
    string Description,
    AptitudeTemplateAptitudes Aptitudes,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);
    
public record AptitudeTemplateAptitudes(
    int Cognition,
    int Intuition,
    int Reflexes,
    int Savvy,
    int Somatics,
    int Willpower);