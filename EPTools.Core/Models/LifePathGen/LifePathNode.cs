using System.Text.Json.Serialization;
using EPTools.Core.Interfaces;

namespace EPTools.Core.Models.LifePathGen;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LifePathNodeType
{
    // Core handler types
    Morph, Skill, Trait, Aptitude, ForcedInterest, Language, Pool,
    Reputation, Sleight, Faction, Age, Motivation, Skip, PlayerChoice,
    Interest, Career, BackgroundOption, CharacterGenStep, Table,
    LifePathStoryEvent, Attribute,

    // Structural meta-types from data files (no handlers; LifepathService ignores these)
    BackgroundAdvancedAge, BackgroundAge, BackgroundNativeTongue, BackgroundPath,
    BackgroundYouthEvent, CareerPath, CharacterGenAptitude, CharacterGenFaction,
    CharacterGenInterest, Disorder, FallEvent, PostFallEvent, PreFallEvent,
    UserChoice, LifePathEventCrime, LifePathEventFirewall, LifePathEventGatecrashing, None,

    // Data file typos — kept as separate members so JSON round-trips cleanly;
    // LifepathService maps these to the same handler as their correct counterparts.
    Slight,  // typo for Sleight in psi sleight table data
    Trail,   // typo for Trait in fall event data

    // Multi-word string values that cannot be plain C# identifiers
    [JsonStringEnumMemberName("Psi Ability")]       PsiAbility,
    [JsonStringEnumMemberName("Reputation Network")] ReputationNetwork,
    [JsonStringEnumMemberName("Skip Age")]           SkipAge,
    [JsonStringEnumMemberName("Skip Fall")]          SkipFall,
    [JsonStringEnumMemberName("Skip Pre-Fall")]      SkipPreFall,
}

public record LifePathNode(
    string Name,
    LifePathNodeType Type,
    string Description,
    int Value,
    int Weight,
    List<List<LifePathNode>> OptionLists
) : IWeightedItem;
