using EPTools.Core.Models.EPDataModels;

#pragma warning disable CS8618

namespace EPTools.Core.Models.Ego
{
    public class InventoryItem
    {
        public Gear Item { get; set; }
        public int Quantity { get; set; }
        public bool Equipped { get; set; }
        public bool Active { get; set; }
        public string Notes { get; set; }
    }
}
