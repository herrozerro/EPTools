using EPTools.Desktop.ViewModels;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.Tests;

public class EgoTraitViewModelTests
{
    [Fact]
    public void Constructor_SetsPropertiesFromModel()
    {
        var trait = new EgoTrait
        {
            Name = "Common Sense",
            Type = "Positive",
            Level = 1,
            Cost = 10,
            CostTiers = "10/20/30",
            Summary = "GM hint when doing something dumb",
            Description = "Full description here"
        };

        var vm = new EgoTraitViewModel(trait);

        Assert.Equal("Common Sense", vm.Name);
        Assert.Equal("Positive", vm.Type);
        Assert.Equal(1, vm.Level);
        Assert.Equal(10, vm.Cost);
        Assert.Equal("10/20/30", vm.CostTiers);
        Assert.Equal("GM hint when doing something dumb", vm.Summary);
        Assert.Equal("Full description here", vm.Description);
    }

    [Fact]
    public void Properties_UpdateModel()
    {
        var trait = new EgoTrait();
        var vm = new EgoTraitViewModel(trait);

        vm.Name = "Updated Name";
        vm.Type = "Negative";
        vm.Level = 2;
        vm.Cost = 20;
        vm.CostTiers = "20/40";
        vm.Summary = "Updated Summary";
        vm.Description = "Updated Description";

        Assert.Equal("Updated Name", trait.Name);
        Assert.Equal("Negative", trait.Type);
        Assert.Equal(2, trait.Level);
        Assert.Equal(20, trait.Cost);
        Assert.Equal("20/40", trait.CostTiers);
        Assert.Equal("Updated Summary", trait.Summary);
        Assert.Equal("Updated Description", trait.Description);
    }

    [Fact]
    public void Model_ReturnsUnderlyingTrait()
    {
        var trait = new EgoTrait { Name = "Test" };
        var vm = new EgoTraitViewModel(trait);

        Assert.Same(trait, vm.Model);
    }

    [Fact]
    public void TypeDisplay_ReturnsCorrectSymbol()
    {
        var positiveTrait = new EgoTraitViewModel(new EgoTrait { Type = "Positive" });
        var negativeTrait = new EgoTraitViewModel(new EgoTrait { Type = "Negative" });
        var neutralTrait = new EgoTraitViewModel(new EgoTrait { Type = "Neutral" });
        var unknownTrait = new EgoTraitViewModel(new EgoTrait { Type = "Something Else" });

        Assert.Equal("+", positiveTrait.TypeDisplay);
        Assert.Equal("-", negativeTrait.TypeDisplay);
        Assert.Equal("~", neutralTrait.TypeDisplay);
        Assert.Equal("~", unknownTrait.TypeDisplay);
    }

    [Fact]
    public void AvailableTypes_ContainsExpectedTypes()
    {
        var types = EgoTraitViewModel.AvailableTypes;

        Assert.Contains("Positive", types);
        Assert.Contains("Negative", types);
        Assert.Contains("Neutral", types);
        Assert.Equal(3, types.Count);
    }

    [Fact]
    public void PropertyChanged_RaisedForName()
    {
        var trait = new EgoTrait { Name = "Original" };
        var vm = new EgoTraitViewModel(trait);
        var raised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoTraitViewModel.Name))
                raised = true;
        };

        vm.Name = "Updated";

        Assert.True(raised);
    }

    [Fact]
    public void PropertyChanged_RaisedForType()
    {
        var trait = new EgoTrait { Type = "Positive" };
        var vm = new EgoTraitViewModel(trait);
        var raised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoTraitViewModel.Type))
                raised = true;
        };

        vm.Type = "Negative";

        Assert.True(raised);
    }

    [Fact]
    public void PropertyChanged_RaisedForLevel()
    {
        var trait = new EgoTrait { Level = 1 };
        var vm = new EgoTraitViewModel(trait);
        var raised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoTraitViewModel.Level))
                raised = true;
        };

        vm.Level = 2;

        Assert.True(raised);
    }

    [Fact]
    public void PropertyChanged_RaisedForCost()
    {
        var trait = new EgoTrait { Cost = 10 };
        var vm = new EgoTraitViewModel(trait);
        var raised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoTraitViewModel.Cost))
                raised = true;
        };

        vm.Cost = 20;

        Assert.True(raised);
    }
}
