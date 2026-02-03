using EPTools.Core.Interfaces;

namespace EPTools.Core.Services;

/// <summary>
/// Default implementation of IRandomizer using Random.Shared for thread-safe random number generation.
/// </summary>
public class DefaultRandomizer : IRandomizer
{
    public int Next(int minValue, int maxValue) => Random.Shared.Next(minValue, maxValue);
}