using System.Text.Json.Serialization;
using EPTools.Core.Models.EPDataModels;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Models.Ego
{
    public class Ego
    {
        public Ego()
        {
            Identities.Add(new Identity{ Alias = "Default Alias"});
        }
        
        public Guid Id { get; set; } = Guid.NewGuid();
        
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
        [JsonPropertyOrder(22)]
        public List<EgoAptitude> Aptitudes { get; set; } = [];

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


        public int SkillTotal(EgoSkill skill)
        {
            var skillAttribute = Aptitudes.Find(x => x.Name == skill.Aptitude);
            if (skillAttribute == null) return skill.Rank;
            return skill.Rank + skillAttribute.AptitudeValue;
        }
    }

    public class EgoAptitude
    {
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public int AptitudeValue { get; set; }
        public int CheckMod { get; set; }
        
        public int CheckRating => AptitudeValue * 3 + CheckMod;
    }
}
