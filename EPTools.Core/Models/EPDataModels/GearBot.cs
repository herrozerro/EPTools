using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public class GearBot : Gear
    {
        public int Vigor { get; init; }
        public int Vigor2 { get; init; }
        public int Flex { get; init; }
        public int ArmorEnergy { get; init; }
        public int ArmorKinetic { get; init; }
        public int WoundThreshold { get; init; }
        public int Durability { get; init; }
        public int DeathRating { get; init; }
        public List<MorphMovementRates> MovementRate { get; init; } = [];
        public string Size { get; init; }= string.Empty;
        public List<string> Ware { get; init; } = [];
    }
}