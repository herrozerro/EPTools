using System.Text.Json.Serialization;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearArmor(
        [property: JsonPropertyName("waretype")] string WareType,
        [property: JsonPropertyName("energy")] int Energy,
        [property: JsonPropertyName("kinetic")] int Kinetic,
        [property: JsonPropertyName("stackable")] bool Stackable,
        string Category,
        string Subcategory,
        string Name,
        string Complexity,
        string Description,
        string Summary,
        string Resource,
        string Reference,
        string Id
        ) : Gear(Category, Subcategory, Name, Complexity, Description, Summary, Resource, Reference, Id);
}
