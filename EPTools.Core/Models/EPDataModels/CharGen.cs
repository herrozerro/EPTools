using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record CharGen(
        [property: JsonPropertyName("step_name")] string StepName,
        [property: JsonPropertyName("guidance")]CharGenGuidance Guidance,
        [property: JsonPropertyName("resource")]string Resource,
        [property: JsonPropertyName("reference")]string Reference,
        [property: JsonPropertyName("id")]string Id);

    public record CharGenGuidance(
        [property: JsonPropertyName("heading")]string Heading,
        [property: JsonPropertyName("text")]string Text);
}
