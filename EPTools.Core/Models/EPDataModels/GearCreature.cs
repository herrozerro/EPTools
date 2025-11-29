using System.Runtime.CompilerServices;

namespace EPTools.Core.Models.EPDataModels
{
    public class GearCreature : Gear
    {
        public Attributes Attributes { get; set; } = new();
        public List<MorphMovementRates> MovementRate { get; set; } = [];
        public List<string> Ware { get; set; } = [];
        public List<string> Skills { get; set; } = [];
        public List<MorphTrait> Traits { get; set; } = [];
    }
    public class Attributes()
    {
        public int Cognition { get; set; }
        public int CognitionCheck { get; set; }
        public int Intuition { get; set; }
        public int IntuitionCheck { get; set; }
        public int Reflexes { get; set; }
        public int ReflexesCheck { get; set; }
        public int Savvy { get; set; }
        public int SavvyCheck { get; set; }
        public int Somatics { get; set; }
        public int SomaticsCheck { get; set; }
        public int Willpower { get; set; }
        public int WillpowerCheck { get; set; }
        public int Initiative { get; set; }
        public int Tp { get; set; }
        public int ArmorEnergy { get; set; }
        public int ArmorKinetic { get; set; }
        public int WoundThreshold { get; set; }
        public int Durability { get; set; }
        public int DeathRating { get; set; }
        public int TraumaThreshold { get; set; }
        public int Lucidity { get; set; }
        public int InsanityRating { get; set; }
    }
}
