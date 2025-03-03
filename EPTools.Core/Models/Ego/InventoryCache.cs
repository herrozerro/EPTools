namespace EPTools.Core.Models.Ego
{
    public class InventoryCache
    {
        public string Location { get; set; } = string.Empty;
        public List<InventoryItem> Inventory { get; set; } = new();
        public List<Morph> Morphs { get; set; } = new();
    }
}
