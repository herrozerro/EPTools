using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record GearBot(
        int Vigor,
        int Vigor2,
        int Flex,
        int ArmorEnergy,
        int ArmorKinetic,
        int WoundThreshold,
        int Durability,
        int DeathRating,
        List<MorphMovementRates> MovementRate,
        string Size,
        List<string> Ware,
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
        public GearBot() : this(
            Vigor: 0,
            Vigor2: 0,
            Flex: 0,
            ArmorEnergy: 0,
            ArmorKinetic: 0,
            WoundThreshold: 0,
            Durability: 0,
            DeathRating: 0,
            MovementRate: [],
            Size: "Medium",       // Default size
            Ware: new List<string>(),
            
            // Base Gear Properties
            Category: "Bot",
            Subcategory: "Custom",
            Name: "New Bot",
            Complexity: "Major",
            Description: "Custom Bot Shell",
            Summary: "",
            Resource: "",
            Reference: "",
            AdditionalRules: []
        ) { }
    }
}