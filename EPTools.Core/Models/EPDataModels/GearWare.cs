namespace EPTools.Core.Models.EPDataModels
{
    public record GearWare(
        bool Bioware,
        bool Cyberware,
        bool Hardware,
        bool Meshware,
        bool Nanoware,
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
