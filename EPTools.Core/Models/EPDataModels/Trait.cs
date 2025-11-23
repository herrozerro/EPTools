using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Trait(
        string Name,
        string Type,
        List<int> Cost,
        bool Ego,
        bool Morph,
        string Summary,
        string Description,
        bool Auto,
        bool Noted,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules
        );
}
