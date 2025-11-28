namespace EPTools.Core.Models.EPDataModels
{
    public record GearCreature(
        Attributes Attributes,
        List<MorphMovementRates> MovementRate,
        List<string> Ware,
        List<string> Skills,
        List<MorphTrait> Traits,
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

    public record Attributes(
        int Cognition,
        int CognitionCheck,
        int Intuition,
        int IntuitionCheck,
        int Reflexes,
        int ReflexesCheck,
        int Savvy,
        int SavvyCheck,
        int Somatics,
        int SomaticsCheck,
        int Willpower,
        int WillpowerCheck,
        int Initiative,
        int Tp,
        int ArmorEnergy,
        int ArmorKinetic,
        int WoundThreshold,
        int Durability,
        int DeathRating,
        int TraumaThreshold,
        int Lucidity,
        int InsanityRating);
}
