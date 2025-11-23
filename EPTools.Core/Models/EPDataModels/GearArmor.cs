using System.Text.Json.Serialization;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearArmor(
        string WareType,
        int Energy,
        int Kinetic,
        bool Stackable,
        string Category,
        string Subcategory,
        string Name,
        string Complexity,
        string Description,
        string Summary,
        string Resource,
        string Reference,
        List<AdditionalRules> AdditionalRules
        ) : Gear(Category, Subcategory, Name, Complexity, Description, Summary, Resource, Reference, AdditionalRules);
}
