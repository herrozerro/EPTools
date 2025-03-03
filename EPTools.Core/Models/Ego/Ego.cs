using System.Text.Json.Serialization;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Models.Ego
{
    public class Ego
    {
        //General Information
        [JsonPropertyOrder(0)] 
        public string Name { get; set; } = string.Empty;
        [JsonPropertyOrder(1)]
        public string EgoSex { get; set; } = string.Empty;
        [JsonPropertyOrder(2)]
        public string EgoGender { get; set; } = string.Empty;
        [JsonPropertyOrder(4)]
        public int EgoAge { get; set; }
        [JsonPropertyOrder(5)]
        public string Background { get; set; } = string.Empty;
        [JsonPropertyOrder(6)]
        public string Career { get; set; } = string.Empty;
        [JsonPropertyOrder(7)]
        public string Interest { get; set; } = string.Empty;
        [JsonPropertyOrder(8)]
        public string Faction { get; set; } = string.Empty;
        [JsonPropertyOrder(9)]
        public string Languages { get; set; } = string.Empty;
        [JsonPropertyOrder(10)]
        public List<string> Motivations { get; set; } = [];

        //Aptitudes
        [JsonPropertyOrder(11)]
        public int Cognition { get; set; }
        [JsonPropertyOrder(12)]
        public int CognitionCheckMod { get; set; }
        [JsonPropertyOrder(13)]
        public int Intuition { get; set; }
        [JsonPropertyOrder(14)]
        public int IntuitionCheckMod { get; set; }
        [JsonPropertyOrder(15)]
        public int Reflex { get; set; }
        [JsonPropertyOrder(16)]
        public int ReflexCheckMod { get; set; }
        [JsonPropertyOrder(17)]
        public int Savvy { get; set; }
        [JsonPropertyOrder(18)]
        public int SavvyCheckMod { get; set; }
        [JsonPropertyOrder(19)]
        public int Somatics { get; set; }
        [JsonPropertyOrder(20)]
        public int SomaticsCheckMod { get; set; }
        [JsonPropertyOrder(21)]
        public int Willpower { get; set; }
        [JsonPropertyOrder(22)]
        public int WillpowerCheckMod { get; set; }
        [JsonPropertyOrder(23)]
        public int EgoFlex { get; set; }

        //Identity
        [JsonPropertyOrder(24)]
        public List<Identity> Identities { get; set; } = [];

        //Ego Traits
        [JsonPropertyOrder(25)]
        public List<Trait> EgoTraits { get; set; } = [];

        //Skills
        [JsonPropertyOrder(26)]
        public List<EgoSkill> Skills { get; set; } = [];

        //PSI
        [JsonPropertyOrder(27)]
        public string Substrain { get; set; } = string.Empty;
        [JsonPropertyOrder(28)]
        public int InfectionRating { get; set; }
        [JsonPropertyOrder(29)]
        public List<string> InfectionEvents { get; set; } = [];
        [JsonPropertyOrder(30)]
        public List<Slight> Slights { get; set; } = [];

        //Morph
        [JsonPropertyOrder(31)]
        public List<Morph> Morphs { get; set; } = [];

        //Local Inventory
        [JsonPropertyOrder(32)]
        public List<InventoryItem> Inventory { get; set; } = [];
        
        //Inventory Caches
        [JsonPropertyOrder(33)]
        public List<InventoryCache> Caches { get; set; } = [];

        //Notes
        [JsonPropertyOrder(34)]
        public string Notes { get; set; } = string.Empty;
        
        //lifepath Tracking Variables
        [JsonIgnore]
        public List<int> SkipSections { get; set; } = [];
        
        [JsonIgnore]
        public Stack<LifePathNode> CharacterGenerationNodes { get; set; } = new();
        
        [JsonPropertyOrder(35)]
        public List<string> CharacterGenerationOutput { get; set; } = [];
        
        [JsonPropertyOrder(36)]
        public List<string> PlayerChoices { get; set; } = [];
    }
}
