using EPTools.Core.Interfaces;

namespace EPTools.Core.Models.LifePathGen;

public record RandomTable(
    string TableName,
    int Weight,
    string Value) : IWeightedItem;