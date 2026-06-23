namespace EPTools.Core.Models.Data;

public record CharGen(
    string StepName,
    CharGenGuidance Guidance,
    string Resource,
    string Reference,
    List<AdditionalRules> AdditionalRules);

public record CharGenGuidance(
    string Heading,
    string Text);