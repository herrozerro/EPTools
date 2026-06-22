using System.Text.Json.Serialization;
using EPTools.Core.Interfaces;

namespace EPTools.Core.Models.LifePathGen;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LifePathNodeType
{
    Morph, Skill, Trait, Aptitude, ForcedInterest, Language, Pool,
    Reputation, Sleight, Faction, Age, Motivation, Skip, PlayerChoice,
    Interest, Career, BackgroundOption, CharacterGenStep, Table,
    LifePathStoryEvent, Attribute
}

public record LifePathNode(
    string Name,
    LifePathNodeType Type,
    string Description,
    int Value,
    int Weight,
    List<List<LifePathNode>> OptionLists
) : IWeightedItem;
