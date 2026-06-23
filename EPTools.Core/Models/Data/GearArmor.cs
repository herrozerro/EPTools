namespace EPTools.Core.Models.Data;

public class GearArmor : Gear
{
    public string WareType { get; set; } = string.Empty;
    public int Energy { get; set; }
    public int Kinetic { get; set; }
    public bool Stackable { get; set; }
}