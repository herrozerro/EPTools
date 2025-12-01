#pragma warning disable CS8618

namespace EPTools.Core.Models.Ego;

public class Ware
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
    public int Cost { get; set; }
    public int Quantity { get; set; }
}