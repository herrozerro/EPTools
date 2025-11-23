using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Faction(
            string Name,
            string Description,
            string Resource,
            string Reference,
            string Id);
}
