using EPTools.Core.Models.EPDataModels;

namespace EPTools.Core.Models.Ego;

public class InventoryItem
{
    // Instance Data (Mutable)
    public Guid InstanceId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public bool Equipped { get; set; }
    public bool Active { get; set; }
    public string Notes { get; set; } = string.Empty;

    // The Data Reference (Immutable)
    // This holds the actual stats, category, rules, etc.
    // Nullable because items can exist without a base gear template (custom items)
    public Gear? BaseGear { get; set; }
}