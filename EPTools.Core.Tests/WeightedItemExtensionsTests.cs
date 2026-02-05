using EPTools.Core.Extensions;
using EPTools.Core.Interfaces;

namespace EPTools.Core.Tests;

public class WeightedItemExtensionsTests
{
    private class TestWeightedItem(string name, int weight) : IWeightedItem
    {
        public string Name { get; } = name;
        public int Weight { get; } = weight;
    }

    private class MockRandomizer(int returnValue) : IRandomizer
    {
        public int Next(int minValue, int maxValue) => returnValue;
    }

    [Fact]
    public void GetWeightedItem_EmptyList_ThrowsArgumentException()
    {
        var items = new List<TestWeightedItem>();

        Assert.Throws<ArgumentException>(() => items.GetWeightedItem(new MockRandomizer(0)));
    }

    [Fact]
    public void GetWeightedItem_SingleItem_ReturnsThatItem()
    {
        var items = new List<TestWeightedItem>
        {
            new("Only", 10)
        };

        var result = items.GetWeightedItem(new MockRandomizer(0));

        Assert.Equal("Only", result.Name);
    }

    [Fact]
    public void GetWeightedItem_SelectsFirstItem_WhenRandomBelowFirstWeight()
    {
        var items = new List<TestWeightedItem>
        {
            new("First", 10),
            new("Second", 20),
            new("Third", 30)
        };

        var result = items.GetWeightedItem(new MockRandomizer(5));

        Assert.Equal("First", result.Name);
    }

    [Fact]
    public void GetWeightedItem_SelectsSecondItem_WhenRandomInSecondRange()
    {
        var items = new List<TestWeightedItem>
        {
            new("First", 10),
            new("Second", 20),
            new("Third", 30)
        };

        var result = items.GetWeightedItem(new MockRandomizer(15));

        Assert.Equal("Second", result.Name);
    }

    [Fact]
    public void GetWeightedItem_SelectsThirdItem_WhenRandomInThirdRange()
    {
        var items = new List<TestWeightedItem>
        {
            new("First", 10),
            new("Second", 20),
            new("Third", 30)
        };

        var result = items.GetWeightedItem(new MockRandomizer(35));

        Assert.Equal("Third", result.Name);
    }

    [Fact]
    public void GetWeightedItem_WithModifier_ShiftsTowardHigherWeightedItems()
    {
        var items = new List<TestWeightedItem>
        {
            new("First", 10),
            new("Second", 10),
            new("Third", 10)
        };

        // Random returns 0, but modifier of 25 pushes past total weight (30),
        // so it should return the last item
        var result = items.GetWeightedItem(new MockRandomizer(0), 25);

        Assert.Equal("Third", result.Name);
    }

    [Fact]
    public void GetWeightedItem_NegativeModifier_IsTreatedAsZero()
    {
        var items = new List<TestWeightedItem>
        {
            new("First", 10),
            new("Second", 10)
        };

        // Negative modifier is clamped to 0 via Math.Max
        var result = items.GetWeightedItem(new MockRandomizer(0), -5);

        Assert.Equal("First", result.Name);
    }

    [Fact]
    public void GetWeightedItem_SelectsAtBoundary_ReturnsCorrectItem()
    {
        var items = new List<TestWeightedItem>
        {
            new("First", 10),
            new("Second", 10)
        };

        // Random returns exactly at the boundary of first item's weight
        var result = items.GetWeightedItem(new MockRandomizer(10));

        Assert.Equal("Second", result.Name);
    }
}
