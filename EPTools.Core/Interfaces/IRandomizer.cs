namespace EPTools.Core.Interfaces;

/// <summary>
/// Abstraction for random number generation to enable deterministic testing.
/// </summary>
public interface IRandomizer
{
    /// <summary>
    /// Returns a random integer within the specified range.
    /// </summary>
    /// <param name="minValue">The inclusive lower bound.</param>
    /// <param name="maxValue">The exclusive upper bound.</param>
    /// <returns>A random integer >= minValue and less than maxValue.</returns>
    int Next(int minValue, int maxValue);
}