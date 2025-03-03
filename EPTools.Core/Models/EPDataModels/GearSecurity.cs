namespace EPTools.Core.Models.EPDataModels
{
    public record GearSecurity(
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
