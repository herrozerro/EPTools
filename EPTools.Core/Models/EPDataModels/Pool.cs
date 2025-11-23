using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Pool(
        string Name,
        List<string> Effects,
        List<string> Checks,
        string Resource,
        string Reference,
        string Id);
}
