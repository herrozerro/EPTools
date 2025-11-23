using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record CharGen(
        string StepName,
        CharGenGuidance Guidance,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules);

    public record CharGenGuidance(
        string Heading,
        string Text);
}
