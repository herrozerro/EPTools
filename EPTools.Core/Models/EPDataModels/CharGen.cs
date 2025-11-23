using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record CharGen(
        string StepName,
        CharGenGuidance Guidance,
        string Resource,
        string Reference,
        string Id);

    public record CharGenGuidance(
        string Heading,
        string Text);
}
