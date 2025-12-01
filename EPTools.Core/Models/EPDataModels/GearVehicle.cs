namespace EPTools.Core.Models.EPDataModels;

public class GearVehicle : Gear
{
    public int Passengers { get; set; }
    public int Vigor { get; set; }
    public int Flex { get; set; }
    public int ArmorEnergy { get; set; }
    public int ArmorKinetic { get; set; }
    public int WoundThreshold { get; set; }
    public int Durability { get; set; }
    public int DeathRating { get; set; }
    public List<MorphMovementRates> MovementRate { get; set; } = [];
    public string Size { get; set; } = string.Empty;
    public List<string> Ware { get; set; } = [];
}