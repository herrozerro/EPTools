using System.Text.Json.Serialization;
using EPTools.Core.Constants;

namespace EPTools.Core.Models.Ego;

public class Ego
{
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
    public List<string> Languages { get; set; } = [];
    [JsonPropertyOrder(10)]
    public List<string> Motivations { get; set; } = [];

    private List<RollModifier> RollModifiers { get; set; } = [];
    
    public int RezEarned { get; set; }
    public int RezSpent { get; set; }
    
    //Derived Stats
    public int Initiative
    {
        get
        {
            var reflexes = Aptitudes.Find(x => x.Name == AptitudeNames.Reflexes)?.AptitudeValue ?? 0;
            var intuition = Aptitudes.Find(x => x.Name == AptitudeNames.Intuition)?.AptitudeValue ?? 0;
            return (reflexes + intuition)/5 + RollModifiers.Where(x=>x.Type == RollModifierType.Initiative).Sum(x => x.Value);
        }
    }

    //Mental Health
    public int Stress { get; set; }
    public int Lucidity
    {
        get
        {
            var willpower = Aptitudes.Find(x => x.Name == AptitudeNames.Willpower)?.AptitudeValue ?? 0;
            return (willpower * 2) + RollModifiers.Where(x=>x.Type == RollModifierType.Lucidity).Sum(x => x.Value);
        }
    }
    public int TraumaThreshold
    {
        get
        {
            return (Lucidity / 5) + RollModifiers.Where(x=> x.Type == RollModifierType.TraumaThreshold).Sum(x => x.Value);
        }
    }
    public int InsanityRating
    {
        get
        {
            return (Lucidity * 2) + RollModifiers.Where(x=> x.Type == RollModifierType.InsanityRating).Sum(x => x.Value);
        }
    }


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
    public List<EgoTrait> EgoTraits { get; set; } = [];

    //Skills
    [JsonPropertyOrder(26)]
    public List<EgoSkill> Skills { get; set; } = [];

    //PSI
    [JsonPropertyOrder(27)]
    public EgoPsi Psi { get; set; } = new();

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
        
    [JsonPropertyOrder(35)]
    public List<string> CharacterGenerationOutput { get; set; } = [];
        
    [JsonPropertyOrder(36)]
    public List<string> PlayerChoices { get; set; } = [];


    public int SkillTotal(EgoSkill skill)
    {
        var skillAttributeValue = Aptitudes.Find(x => x.Name == skill.Aptitude)?.AptitudeValue ?? 0;
        return skill.Rank + skillAttributeValue;
    }
}