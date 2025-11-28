namespace EPTools.Core.Models.EPDataModels
{
    public record Faction(
            string Name,
            string Description,
            string Resource,
            string Reference,
            List<AdditionalRules> AdditionalRules);
}
