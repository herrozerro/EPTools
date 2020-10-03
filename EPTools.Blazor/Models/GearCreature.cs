using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class GearCreature : Gear
    {
        public Attributes attributes { get; set; }
        public MorphMovementRates movement_rate { get; set; }
        public List<string> ware { get; set; }
        public List<string> skils { get; set; }
        public MorphTrait traits { get; set; }
    }

    public class Attributes
    {
        public int cognition { get; set; }
        public int cognition_check { get; set; }
        public int intuition { get; set; }
        public int intuition_check { get; set; }
        public int reflexes { get; set; }
        public int reflexes_check { get; set; }
        public int savvy { get; set; }
        public int savvy_check { get; set; }
        public int somatics { get; set; }
        public int somatics_check { get; set; }
        public int willpower { get; set; }
        public int willpower_check { get; set; }
        public int initiative { get; set; }
        public int tp { get; set; }
        public int armor_energy { get; set; }
        public int armor_kenetic { get; set; }
        public int wound_threshold { get; set; }
        public int durability { get; set; }
        public int death_rating { get; set; }
        public int trauma_threshold { get; set; }
        public int lucidity { get; set; }
        public int insanity_rating { get; set; }
    }
}
