using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearCategories(
        string Name,
        string Text,
        GearSubcategories Subcategories,
        string Id,
        string Reference,
        string Resource);

    public record GearSubcategories(
        string Name,
        string Text);
}
