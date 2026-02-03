namespace EPTools.Core.Models.Ego;

public class EgoTrait
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Cost { get; set; }
    public string CostTiers { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<AdditionalRules> AdditionalRules { get; set; } = [];
}