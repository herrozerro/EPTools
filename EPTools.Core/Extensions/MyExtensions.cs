﻿using EPTools.Core.Interfaces;

namespace EPTools.Core.Extensions
{
    public static class MyExtensions
    {
        public static T GetWeightedItem<T>(this IEnumerable<T> weightedList, int weightedListModifier = 0) where T : IWeightedItem
        {
            if (weightedList.Count() == 0)
                throw new ArgumentException(nameof(weightedList));
            
            var weightedItems = weightedList.ToList();

            var totalWeight = weightedItems.Sum(x => x.Weight);

            var randomNumber = Random.Shared.Next(0, totalWeight) + Math.Max(weightedListModifier, 0);

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
}
