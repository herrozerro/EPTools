using EPTools.Core.Services;

namespace EPTools.Core.Tests;

public class DefaultRandomizerTests
{
    private readonly DefaultRandomizer _randomizer = new();

    [Fact]
    public void Next_ReturnsValueWithinRange()
    {
        for (var i = 0; i < 100; i++)
        {
            var result = _randomizer.Next(0, 10);
            Assert.InRange(result, 0, 9);
        }
    }

    [Fact]
    public void Next_WithSameMinMax_ReturnsMinValue()
    {
        var result = _randomizer.Next(5, 5);
        Assert.Equal(5, result);
    }

    [Fact]
    public void Next_WithNegativeRange_ReturnsValueWithinRange()
    {
        for (var i = 0; i < 100; i++)
        {
            var result = _randomizer.Next(-10, 0);
            Assert.InRange(result, -10, -1);
        }
    }
}
