using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record MorphType(
        string Name,
        string Description,
        bool Biological,
        string Resource,
        string Reference,
        string Id);
}
