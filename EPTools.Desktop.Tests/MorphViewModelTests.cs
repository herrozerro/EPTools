using EPTools.Desktop.ViewModels;
using EPTools.Core.Models.Ego;
using Xunit;

namespace EPTools.Desktop.Tests;

public class MorphViewModelTests
{
    [Fact]
    public void Constructor_SetsPropertiesFromModel()
    {
        var morph = new Morph
        {
            Name = "Splicer",
            MorphType = "Biomorph",
            MorphSex = "Female",
            Size = "Medium",
            Vigor = 1,
            Insight = 0,
            Moxie = 1,
            MorphFlex = 0,
            ActiveMorph = true
        };

        var vm = new MorphViewModel(morph);

        Assert.Equal("Splicer", vm.Name);
        Assert.Equal("Biomorph", vm.MorphType);
        Assert.Equal("Female", vm.MorphSex);
        Assert.Equal("Medium", vm.Size);
        Assert.Equal(1, vm.Vigor);
        Assert.Equal(0, vm.Insight);
        Assert.Equal(1, vm.Moxie);
        Assert.Equal(0, vm.MorphFlex);
        Assert.True(vm.ActiveMorph);
    }

    [Fact]
    public void Name_UpdatesModel()
    {
        var morph = new Morph { Name = "Original" };
        var vm = new MorphViewModel(morph);

        vm.Name = "Updated";

        Assert.Equal("Updated", morph.Name);
    }

    [Fact]
    public void MorphType_UpdatesModel()
    {
        var morph = new Morph { MorphType = "Biomorph" };
        var vm = new MorphViewModel(morph);

        vm.MorphType = "Synthmorph";

        Assert.Equal("Synthmorph", morph.MorphType);
    }

    [Fact]
    public void Pools_UpdateModel()
    {
        var morph = new Morph();
        var vm = new MorphViewModel(morph);

        vm.Vigor = 2;
        vm.Insight = 3;
        vm.Moxie = 1;
        vm.MorphFlex = 2;

        Assert.Equal(2, morph.Vigor);
        Assert.Equal(3, morph.Insight);
        Assert.Equal(1, morph.Moxie);
        Assert.Equal(2, morph.MorphFlex);
    }

    [Fact]
    public void ActiveMorph_UpdatesModel()
    {
        var morph = new Morph { ActiveMorph = false };
        var vm = new MorphViewModel(morph);

        vm.ActiveMorph = true;

        Assert.True(morph.ActiveMorph);
    }

    [Fact]
    public void Model_ReturnsUnderlyingMorph()
    {
        var morph = new Morph { Name = "Test" };
        var vm = new MorphViewModel(morph);

        Assert.Same(morph, vm.Model);
    }

    [Fact]
    public void Constructor_CreatesTraitViewModels()
    {
        var morph = new Morph
        {
            Traits = new List<EgoTrait>
            {
                new() { Name = "Trait 1" },
                new() { Name = "Trait 2" }
            }
        };

        var vm = new MorphViewModel(morph);

        Assert.Equal(2, vm.Traits.Count);
        Assert.Equal("Trait 1", vm.Traits[0].Name);
        Assert.Equal("Trait 2", vm.Traits[1].Name);
    }

    [Fact]
    public void Constructor_CreatesWareViewModels()
    {
        var morph = new Morph
        {
            Wares = new List<Ware>
            {
                new() { Name = "Ware 1" },
                new() { Name = "Ware 2" }
            }
        };

        var vm = new MorphViewModel(morph);

        Assert.Equal(2, vm.Wares.Count);
        Assert.Equal("Ware 1", vm.Wares[0].Name);
        Assert.Equal("Ware 2", vm.Wares[1].Name);
    }

    [Fact]
    public void AddTrait_AddsToModelAndCollection()
    {
        var morph = new Morph();
        var vm = new MorphViewModel(morph);

        vm.AddTrait();

        Assert.Single(morph.Traits);
        Assert.Single(vm.Traits);
        Assert.Equal("New Trait", vm.Traits[0].Name);
    }

    [Fact]
    public void RemoveTrait_RemovesFromModelAndCollection()
    {
        var morph = new Morph
        {
            Traits = new List<EgoTrait> { new() { Name = "Trait 1" } }
        };
        var vm = new MorphViewModel(morph);
        var traitVm = vm.Traits[0];

        vm.RemoveTrait(traitVm);

        Assert.Empty(morph.Traits);
        Assert.Empty(vm.Traits);
    }

    [Fact]
    public void AddWare_AddsToModelAndCollection()
    {
        var morph = new Morph();
        var vm = new MorphViewModel(morph);

        vm.AddWare();

        Assert.Single(morph.Wares);
        Assert.Single(vm.Wares);
        Assert.Equal("New Ware", vm.Wares[0].Name);
    }

    [Fact]
    public void RemoveWare_RemovesFromModelAndCollection()
    {
        var morph = new Morph
        {
            Wares = new List<Ware> { new() { Name = "Ware 1" } }
        };
        var vm = new MorphViewModel(morph);
        var wareVm = vm.Wares[0];

        vm.RemoveWare(wareVm);

        Assert.Empty(morph.Wares);
        Assert.Empty(vm.Wares);
    }

    [Fact]
    public void AvailableMorphTypes_ContainsExpectedTypes()
    {
        var types = MorphViewModel.AvailableMorphTypes;

        Assert.Contains("Biomorph", types);
        Assert.Contains("Pod", types);
        Assert.Contains("Synthmorph", types);
        Assert.Contains("Infomorph", types);
    }

    [Fact]
    public void AvailableSizes_ContainsExpectedSizes()
    {
        var sizes = MorphViewModel.AvailableSizes;

        Assert.Contains("Very Small", sizes);
        Assert.Contains("Small", sizes);
        Assert.Contains("Medium", sizes);
        Assert.Contains("Large", sizes);
        Assert.Contains("Very Large", sizes);
    }

    [Fact]
    public void PropertyChanged_RaisedForName()
    {
        var morph = new Morph { Name = "Original" };
        var vm = new MorphViewModel(morph);
        var raised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MorphViewModel.Name))
                raised = true;
        };

        vm.Name = "Updated";

        Assert.True(raised);
    }
}

public class MorphTraitViewModelTests
{
    [Fact]
    public void Constructor_SetsPropertiesFromModel()
    {
        var trait = new EgoTrait
        {
            Name = "Enhanced Vision",
            Level = 2,
            Cost = 10,
            Summary = "See better"
        };

        var vm = new MorphTraitViewModel(trait);

        Assert.Equal("Enhanced Vision", vm.Name);
        Assert.Equal(2, vm.Level);
        Assert.Equal(10, vm.Cost);
        Assert.Equal("See better", vm.Summary);
    }

    [Fact]
    public void Properties_UpdateModel()
    {
        var trait = new EgoTrait();
        var vm = new MorphTraitViewModel(trait);

        vm.Name = "New Trait";
        vm.Level = 3;
        vm.Cost = 15;
        vm.Summary = "New summary";

        Assert.Equal("New Trait", trait.Name);
        Assert.Equal(3, trait.Level);
        Assert.Equal(15, trait.Cost);
        Assert.Equal("New summary", trait.Summary);
    }

    [Fact]
    public void Model_ReturnsUnderlyingTrait()
    {
        var trait = new EgoTrait { Name = "Test" };
        var vm = new MorphTraitViewModel(trait);

        Assert.Same(trait, vm.Model);
    }
}

public class MorphWareViewModelTests
{
    [Fact]
    public void Constructor_SetsPropertiesFromModel()
    {
        var ware = new Ware
        {
            Name = "Cortical Stack",
            Description = "Backup brain",
            Active = true,
            Cost = 1,
            Quantity = 1
        };

        var vm = new MorphWareViewModel(ware);

        Assert.Equal("Cortical Stack", vm.Name);
        Assert.Equal("Backup brain", vm.Description);
        Assert.True(vm.Active);
        Assert.Equal(1, vm.Cost);
        Assert.Equal(1, vm.Quantity);
    }

    [Fact]
    public void Properties_UpdateModel()
    {
        var ware = new Ware();
        var vm = new MorphWareViewModel(ware);

        vm.Name = "New Ware";
        vm.Description = "New description";
        vm.Active = true;
        vm.Cost = 5;
        vm.Quantity = 2;

        Assert.Equal("New Ware", ware.Name);
        Assert.Equal("New description", ware.Description);
        Assert.True(ware.Active);
        Assert.Equal(5, ware.Cost);
        Assert.Equal(2, ware.Quantity);
    }

    [Fact]
    public void Model_ReturnsUnderlyingWare()
    {
        var ware = new Ware { Name = "Test" };
        var vm = new MorphWareViewModel(ware);

        Assert.Same(ware, vm.Model);
    }
}
