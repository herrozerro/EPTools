using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Aptitude(
        string Name,
        string Description,
        string ShortName,
        string Resource,
        string Reference,
        string Id);
}
