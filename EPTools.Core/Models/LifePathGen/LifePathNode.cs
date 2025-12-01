using EPTools.Core.Interfaces;

namespace EPTools.Core.Models.LifePathGen;

public record LifePathNode(
    string Name, 
    string Type, 
    string Description, 
    int Value, 
    int Weight, 
    List<List<LifePathNode>> OptionLists
) : IWeightedItem;