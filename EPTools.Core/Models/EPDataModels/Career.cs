using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Career(
        string Name,
        string Description,
        List<InterestSkill> Skills,
        string Resource,
        string Reference,
        string Id);
}
