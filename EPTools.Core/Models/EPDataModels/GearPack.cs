using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearPack(
        [property: JsonPropertyName("name")]int Name,
        [property: JsonPropertyName("type")]int Type,
        [property: JsonPropertyName("gear")]List<string> Gear,
        [property: JsonPropertyName("options")]List<GearPackOption> Options,
        [property: JsonPropertyName("resource")]int Resource,
        [property: JsonPropertyName("reference")]int Reference,
        [property: JsonPropertyName("id")]int Id
        );

    public record GearPackOption(
        [property: JsonPropertyName("name")]int Name,
        [property: JsonPropertyName("gear")]List<string> Gear
        );
}
