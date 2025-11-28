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
    ) : Gear(Category, Subcategory, Name, Complexity, Description, Summary, Resource, Reference, AdditionalRules)
    {
        public GearArmor() : this(
            WareType: "",         // Default specific prop
            Energy: 0,            // Default specific prop
            Kinetic: 0,           // Default specific prop
            Stackable: false,     // Default specific prop
            Category: "Armor",    // Force Category
            Subcategory: "",
            Name: "New Armor",
            Complexity: "Minor",
            Description: "Custom Armor",
            Summary: "",
            Resource: "",
            Reference: "",
            AdditionalRules: []
        ) { }
    }
}