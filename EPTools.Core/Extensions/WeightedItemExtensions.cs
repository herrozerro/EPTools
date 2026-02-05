using EPTools.Core.Interfaces;

namespace EPTools.Core.Extensions;

public static class WeightedItemExtensions
{
    /// <summary>
    /// Selects a random item from a weighted list using the provided randomizer.
    /// </summary>
    /// <typeparam name="T">Type implementing IWeightedItem</typeparam>
    /// <param name="weightedList">The list of weighted items to select from</param>
    /// <param name="randomizer">Randomizer for generating random numbers</param>
    /// <param name="weightedListModifier">Optional modifier to shift selection toward higher-weighted items</param>
    /// <returns>A randomly selected item based on weight</returns>
    public static T GetWeightedItem<T>(
        this IEnumerable<T> weightedList,
        IRandomizer randomizer,
        int weightedListModifier = 0) where T : IWeightedItem
    {

        var weightedItems = weightedList.ToList();
        if (weightedItems.Count == 0)
            throw new ArgumentException("Weighted list cannot be empty.", nameof(weightedList));

        var totalWeight = weightedItems.Sum(x => x.Weight);

        var randomNumber = randomizer.Next(0, totalWeight) + Math.Max(weightedListModifier, 0);

        if (randomNumber >= totalWeight)
        {
            return weightedItems.Last();
        }

        foreach (var item in weightedItems)
        {
            if (randomNumber < item.Weight)
            {
                return item;
            }
            randomNumber -= item.Weight;
        }

        throw new InvalidOperationException("No item found in collection for that weight.");
    }
}