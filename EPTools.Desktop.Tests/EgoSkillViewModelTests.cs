using System.Collections.ObjectModel;
using EPTools.Desktop.ViewModels;
using EPTools.Core.Models.Ego;

namespace EPTools.Desktop.Tests;

public class EgoSkillViewModelTests
{
    private readonly ObservableCollection<EgoAptitudeViewModel> _aptitudes;

    public EgoSkillViewModelTests()
    {
        _aptitudes = new ObservableCollection<EgoAptitudeViewModel>
        {
            new(new EgoAptitude { Name = "Cognition", AptitudeValue = 15 }),
            new(new EgoAptitude { Name = "Intuition", AptitudeValue = 10 }),
            new(new EgoAptitude { Name = "Reflexes", AptitudeValue = 20 }),
            new(new EgoAptitude { Name = "Savvy", AptitudeValue = 12 }),
            new(new EgoAptitude { Name = "Somatics", AptitudeValue = 18 }),
            new(new EgoAptitude { Name = "Willpower", AptitudeValue = 8 })
        };
    }

    [Fact]
    public void Constructor_SetsModelAndAptitudes()
    {
        var skill = new EgoSkill { Name = "Athletics", Rank = 40, Aptitude = "Somatics" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.Equal("Athletics", vm.Name);
        Assert.Equal(40, vm.Rank);
        Assert.Equal("Somatics", vm.Aptitude);
    }

    [Fact]
    public void Total_CalculatesRankPlusAptitude()
    {
        var skill = new EgoSkill { Name = "Athletics", Rank = 40, Aptitude = "Somatics" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        // Somatics = 18, Rank = 40, Total should be 58
        Assert.Equal(58, vm.Total);
    }

    [Fact]
    public void Total_UpdatesWhenRankChanges()
    {
        var skill = new EgoSkill { Name = "Athletics", Rank = 40, Aptitude = "Somatics" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.Equal(58, vm.Total);

        vm.Rank = 50;

        Assert.Equal(68, vm.Total);
    }

    [Fact]
    public void Total_UpdatesWhenAptitudeChanges()
    {
        var skill = new EgoSkill { Name = "New Skill", Rank = 30, Aptitude = "Intuition", SkillType = SkillType.KnowledgeSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        // Intuition = 10, Rank = 30, Total = 40
        Assert.Equal(40, vm.Total);

        vm.Aptitude = "Cognition";

        // Cognition = 15, Rank = 30, Total = 45
        Assert.Equal(45, vm.Total);
    }

    [Fact]
    public void Total_UpdatesWhenAptitudeValueChanges()
    {
        var skill = new EgoSkill { Name = "Athletics", Rank = 40, Aptitude = "Somatics" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.Equal(58, vm.Total);

        var somaticsApt = _aptitudes.First(a => a.Name == "Somatics");
        somaticsApt.AptitudeValue = 25;

        Assert.Equal(65, vm.Total);
    }

    [Fact]
    public void Total_ReturnsRankOnly_WhenAptitudeNotFound()
    {
        var skill = new EgoSkill { Name = "Unknown Skill", Rank = 30, Aptitude = "NonExistentAptitude" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.Equal(30, vm.Total);
    }

    [Fact]
    public void CanEditAptitude_FalseForEgoSkill()
    {
        var skill = new EgoSkill { Name = "Athletics", SkillType = SkillType.EgoSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.False(vm.CanEditAptitude);
    }

    [Fact]
    public void CanEditAptitude_TrueForKnowledgeSkill()
    {
        var skill = new EgoSkill { Name = "Academics: Physics", SkillType = SkillType.KnowledgeSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.True(vm.CanEditAptitude);
    }

    [Fact]
    public void CanEditAptitude_TrueForExoticSkill()
    {
        var skill = new EgoSkill { Name = "Pilot: Spacecraft", SkillType = SkillType.ExoticSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.True(vm.CanEditAptitude);
    }

    [Fact]
    public void CanDelete_FalseForEgoSkill()
    {
        var skill = new EgoSkill { Name = "Athletics", SkillType = SkillType.EgoSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.False(vm.CanDelete);
    }

    [Fact]
    public void CanDelete_TrueForKnowledgeSkill()
    {
        var skill = new EgoSkill { Name = "Academics: Physics", SkillType = SkillType.KnowledgeSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.True(vm.CanDelete);
    }

    [Fact]
    public void CanDelete_TrueForExoticSkill()
    {
        var skill = new EgoSkill { Name = "Pilot: Spacecraft", SkillType = SkillType.ExoticSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.True(vm.CanDelete);
    }

    [Fact]
    public void SkillTypeDisplay_ReturnsCorrectLabels()
    {
        var activeSkill = new EgoSkillViewModel(new EgoSkill { SkillType = SkillType.EgoSkill }, _aptitudes);
        var knowSkill = new EgoSkillViewModel(new EgoSkill { SkillType = SkillType.KnowledgeSkill }, _aptitudes);
        var exoticSkill = new EgoSkillViewModel(new EgoSkill { SkillType = SkillType.ExoticSkill }, _aptitudes);

        Assert.Equal("Active", activeSkill.SkillTypeDisplay);
        Assert.Equal("Know", knowSkill.SkillTypeDisplay);
        Assert.Equal("Exotic", exoticSkill.SkillTypeDisplay);
    }

    [Fact]
    public void ExoticAvailableAptitudes_ContainsAllAptitudes()
    {
        var available = EgoSkillViewModel.ExoticAvailableAptitudes;

        Assert.Contains("Cognition", available);
        Assert.Contains("Intuition", available);
        Assert.Contains("Reflexes", available);
        Assert.Contains("Savvy", available);
        Assert.Contains("Somatics", available);
        Assert.Contains("Willpower", available);
        Assert.Equal(6, available.Count);
    }
    
    [Fact]
    public void KnowledgeAvailableAptitudes_ContainsAllAptitudes()
    {
        var available = EgoSkillViewModel.KnowledgeAvailableAptitudes;

        Assert.Contains("Cognition", available);
        Assert.Contains("Intuition", available);
        Assert.Equal(2, available.Count);
    }

    [Fact]
    public void Name_RaisesPropertyChanged()
    {
        var skill = new EgoSkill { Name = "Old Name", SkillType = SkillType.KnowledgeSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        var propertyChangedRaised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoSkillViewModel.Name))
                propertyChangedRaised = true;
        };

        vm.Name = "New Name";

        Assert.True(propertyChangedRaised);
        Assert.Equal("New Name", vm.Name);
    }

    [Fact]
    public void Rank_RaisesPropertyChanged()
    {
        var skill = new EgoSkill { Name = "Test", Rank = 30 };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        var propertyChangedRaised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoSkillViewModel.Rank))
                propertyChangedRaised = true;
        };

        vm.Rank = 50;

        Assert.True(propertyChangedRaised);
        Assert.Equal(50, vm.Rank);
    }

    [Fact]
    public void Specialization_RaisesPropertyChanged()
    {
        var skill = new EgoSkill { Name = "Test", Specialization = "" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        var propertyChangedRaised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoSkillViewModel.Specialization))
                propertyChangedRaised = true;
        };

        vm.Specialization = "Freerunning";

        Assert.True(propertyChangedRaised);
        Assert.Equal("Freerunning", vm.Specialization);
    }

    [Fact]
    public void Aptitude_RaisesPropertyChanged()
    {
        var skill = new EgoSkill { Name = "Test", Aptitude = "Cognition", SkillType = SkillType.KnowledgeSkill };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        var propertyChangedRaised = false;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(EgoSkillViewModel.Aptitude))
                propertyChangedRaised = true;
        };

        vm.Aptitude = "Intuition";

        Assert.True(propertyChangedRaised);
        Assert.Equal("Intuition", vm.Aptitude);
    }

    [Fact]
    public void Model_ReturnsUnderlyingEgoSkill()
    {
        var skill = new EgoSkill { Name = "Test Skill" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.Same(skill, vm.Model);
    }

    [Fact]
    public void Id_ReturnsModelId()
    {
        var skill = new EgoSkill { Name = "Test Skill" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        Assert.Equal(skill.Id, vm.Id);
    }

    [Fact]
    public void Changes_PersistToUnderlyingModel()
    {
        var skill = new EgoSkill { Name = "Original", Rank = 20, Specialization = "", Aptitude = "Cognition" };
        var vm = new EgoSkillViewModel(skill, _aptitudes);

        vm.Name = "Updated Name";
        vm.Rank = 50;
        vm.Specialization = "Special";
        vm.Aptitude = "Reflexes";

        Assert.Equal("Updated Name", skill.Name);
        Assert.Equal(50, skill.Rank);
        Assert.Equal("Special", skill.Specialization);
        Assert.Equal("Reflexes", skill.Aptitude);
    }
}
