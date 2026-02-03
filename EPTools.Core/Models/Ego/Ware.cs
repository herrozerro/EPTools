namespace EPTools.Core.Models.Ego;

public class Ware
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Active { get; set; }
    public int Cost { get; set; }
    public int Quantity { get; set; }
}