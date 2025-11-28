namespace EPTools.Core.Models.EPDataModels
{
    public record GearDrug(
        string Type,
        string Application,
        string Duration,
        string Addiction,
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
