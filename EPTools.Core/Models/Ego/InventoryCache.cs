namespace EPTools.Core.Models.Ego;

public sealed class InventoryCache
{
    public string Location { get; set; } = string.Empty;
    public List<InventoryItem> Inventory { get; set; } = [];
    public List<Morph> Morphs { get; set; } = [];
}