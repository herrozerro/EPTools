namespace EPTools.Core.Models.EPDataModels
{
    public record GearCategories(
        string Name,
        string Text,
        GearSubcategories Subcategories,
        List<AdditionalRules> AdditionalRules,
        string Reference,
        string Resource);

    public record GearSubcategories(
        string Name,
        string Text);
}
