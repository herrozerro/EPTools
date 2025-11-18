using System.Text.Json.Serialization;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Models.EPDataModels
{
    public abstract record Gear(
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("subcategory")] string Subcategory,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("complexity")] string Complexity,
        [property: JsonPropertyName("description")]string Description,
        [property: JsonPropertyName("summary")]string Summary,
        [property: JsonPropertyName("resource")]string Resource,
        [property: JsonPropertyName("reference")]string Reference,
        [property: JsonPropertyName("id")]string Id);
    
}