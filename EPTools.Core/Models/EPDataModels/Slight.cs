using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Slight(
        string Name,
        string Level,
        string Duration,
        string Action,
        string Summary,
        string Description,
        string Resource,
        string Reference,
        string Id);
}
