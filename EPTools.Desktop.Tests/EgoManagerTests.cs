using EPTools.Core.Models.Ego;
using EPTools.Core.Services;
using Xunit;

namespace EPTools.Desktop.Tests;

public class EgoManagerTests
{
    private readonly EgoManager _manager = new();

    #region Skills Tests

    [Fact]
    public void AddKnowledgeSkill_AddsSkillToEgo()
    {
        var ego = new Ego();

        var skill = _manager.AddKnowledgeSkill(ego, "Academics: Physics", "Cognition");

        Assert.Single(ego.Skills);
        Assert.Equal("Academics: Physics", skill.Name);
        Assert.Equal("Cognition", skill.Aptitude);
        Assert.Equal(SkillType.KnowledgeSkill, skill.SkillType);
    }

    [Fact]
    public void AddKnowledgeSkill_DefaultsToIntuition()
    {
        var ego = new Ego();

        var skill = _manager.AddKnowledgeSkill(ego);

        Assert.Equal("Intuition", skill.Aptitude);
    }

    [Fact]
    public void AddKnowledgeSkill_ThrowsForInvalidAptitude()
    {
        var ego = new Ego();

        Assert.Throws<ArgumentException>(() =>
            _manager.AddKnowledgeSkill(ego, "Test", "Reflexes"));
    }

    [Fact]
    public void AddExoticSkill_AddsSkillToEgo()
    {
        var ego = new Ego();

        var skill = _manager.AddExoticSkill(ego, "Pilot: Spacecraft", "Reflexes");

        Assert.Single(ego.Skills);
        Assert.Equal("Pilot: Spacecraft", skill.Name);
        Assert.Equal("Reflexes", skill.Aptitude);
        Assert.Equal(SkillType.ExoticSkill, skill.SkillType);
    }

    [Fact]
    public void AddExoticSkill_AllowsAnyAptitude()
    {
        var ego = new Ego();

        foreach (var apt in EgoManager.AllAptitudes)
        {
            var skill = _manager.AddExoticSkill(ego, $"Skill with {apt}", apt);
            Assert.Equal(apt, skill.Aptitude);
        }
    }

    [Fact]
    public void RemoveSkill_RemovesKnowledgeSkill()
    {
        var ego = new Ego();
        var skill = _manager.AddKnowledgeSkill(ego);

        var result = _manager.RemoveSkill(ego, skill);

        Assert.True(result);
        Assert.Empty(ego.Skills);
    }

    [Fact]
    public void RemoveSkill_RemovesExoticSkill()
    {
        var ego = new Ego();
        var skill = _manager.AddExoticSkill(ego);

        var result = _manager.RemoveSkill(ego, skill);

        Assert.True(result);
        Assert.Empty(ego.Skills);
    }

    [Fact]
    public void RemoveSkill_ReturnsFalseForActiveSkill()
    {
        var ego = new Ego();
        var skill = new EgoSkill { Name = "Athletics", SkillType = SkillType.EgoSkill };
        ego.Skills.Add(skill);

        var result = _manager.RemoveSkill(ego, skill);

        Assert.False(result);
        Assert.Single(ego.Skills);
    }

    [Fact]
    public void SetSkillAptitude_ChangesKnowledgeSkillAptitude()
    {
        var ego = new Ego();
        var skill = _manager.AddKnowledgeSkill(ego, "Test", "Cognition");

        var result = _manager.SetSkillAptitude(ego, skill, "Intuition");

        Assert.True(result);
        Assert.Equal("Intuition", skill.Aptitude);
    }

    [Fact]
    public void SetSkillAptitude_ReturnsFalseForInvalidKnowledgeAptitude()
    {
        var ego = new Ego();
        var skill = _manager.AddKnowledgeSkill(ego, "Test", "Cognition");

        var result = _manager.SetSkillAptitude(ego, skill, "Reflexes");

        Assert.False(result);
        Assert.Equal("Cognition", skill.Aptitude); // Unchanged
    }

    [Fact]
    public void SetSkillAptitude_AllowsAnyAptitudeForExoticSkill()
    {
        var ego = new Ego();
        var skill = _manager.AddExoticSkill(ego);

        var result = _manager.SetSkillAptitude(ego, skill, "Somatics");

        Assert.True(result);
        Assert.Equal("Somatics", skill.Aptitude);
    }

    [Fact]
    public void SetSkillAptitude_ReturnsFalseForActiveSkill()
    {
        var ego = new Ego();
        var skill = new EgoSkill { Name = "Athletics", SkillType = SkillType.EgoSkill, Aptitude = "Somatics" };
        ego.Skills.Add(skill);

        var result = _manager.SetSkillAptitude(ego, skill, "Reflexes");

        Assert.False(result);
        Assert.Equal("Somatics", skill.Aptitude); // Unchanged
    }

    #endregion

    #region Morphs Tests

    [Fact]
    public void AddMorph_AddsMorphToEgo()
    {
        var ego = new Ego();

        var morph = _manager.AddMorph(ego, "Splicer", "Biomorph", "Medium");

        Assert.Single(ego.Morphs);
        Assert.Equal("Splicer", morph.Name);
        Assert.Equal("Biomorph", morph.MorphType);
        Assert.Equal("Medium", morph.Size);
    }

    [Fact]
    public void AddMorph_FirstMorphIsActive()
    {
        var ego = new Ego();

        var morph = _manager.AddMorph(ego);

        Assert.True(morph.ActiveMorph);
    }

    [Fact]
    public void AddMorph_SubsequentMorphsAreNotActive()
    {
        var ego = new Ego();
        _manager.AddMorph(ego, "First");

        var second = _manager.AddMorph(ego, "Second");

        Assert.False(second.ActiveMorph);
    }

    [Fact]
    public void RemoveMorph_RemovesMorph()
    {
        var ego = new Ego();
        _manager.AddMorph(ego, "First");
        var second = _manager.AddMorph(ego, "Second");

        var result = _manager.RemoveMorph(ego, second);

        Assert.True(result);
        Assert.Single(ego.Morphs);
    }

    [Fact]
    public void RemoveMorph_ReturnsFalseForLastMorph()
    {
        var ego = new Ego();
        var morph = _manager.AddMorph(ego);

        var result = _manager.RemoveMorph(ego, morph);

        Assert.False(result);
        Assert.Single(ego.Morphs);
    }

    [Fact]
    public void RemoveMorph_ActivatesAnotherMorphIfActiveRemoved()
    {
        var ego = new Ego();
        var first = _manager.AddMorph(ego, "First");
        var second = _manager.AddMorph(ego, "Second");
        Assert.True(first.ActiveMorph);

        _manager.RemoveMorph(ego, first);

        Assert.True(second.ActiveMorph);
    }

    [Fact]
    public void SetActiveMorph_ActivatesSpecifiedMorph()
    {
        var ego = new Ego();
        var first = _manager.AddMorph(ego, "First");
        var second = _manager.AddMorph(ego, "Second");

        _manager.SetActiveMorph(ego, second);

        Assert.False(first.ActiveMorph);
        Assert.True(second.ActiveMorph);
    }

    [Fact]
    public void GetActiveMorph_ReturnsActiveMorph()
    {
        var ego = new Ego();
        var first = _manager.AddMorph(ego, "First");
        _manager.AddMorph(ego, "Second");

        var active = _manager.GetActiveMorph(ego);

        Assert.Same(first, active);
    }

    [Fact]
    public void GetActiveMorph_ReturnsNullWhenNoMorphs()
    {
        var ego = new Ego();

        var active = _manager.GetActiveMorph(ego);

        Assert.Null(active);
    }

    #endregion

    #region Morph Traits Tests

    [Fact]
    public void AddMorphTrait_AddsTraitToMorph()
    {
        var morph = new Morph();

        var trait = _manager.AddMorphTrait(morph, "Enhanced Vision");

        Assert.Single(morph.Traits);
        Assert.Equal("Enhanced Vision", trait.Name);
    }

    [Fact]
    public void RemoveMorphTrait_RemovesTraitFromMorph()
    {
        var morph = new Morph();
        var trait = _manager.AddMorphTrait(morph);

        var result = _manager.RemoveMorphTrait(morph, trait);

        Assert.True(result);
        Assert.Empty(morph.Traits);
    }

    #endregion

    #region Morph Ware Tests

    [Fact]
    public void AddMorphWare_AddsWareToMorph()
    {
        var morph = new Morph();

        var ware = _manager.AddMorphWare(morph, "Cortical Stack", 1);

        Assert.Single(morph.Wares);
        Assert.Equal("Cortical Stack", ware.Name);
        Assert.Equal(1, ware.Quantity);
    }

    [Fact]
    public void RemoveMorphWare_RemovesWareFromMorph()
    {
        var morph = new Morph();
        var ware = _manager.AddMorphWare(morph);

        var result = _manager.RemoveMorphWare(morph, ware);

        Assert.True(result);
        Assert.Empty(morph.Wares);
    }

    #endregion

    #region Identity Tests

    [Fact]
    public void AddIdentity_AddsIdentityToEgo()
    {
        var ego = new Ego();

        var identity = _manager.AddIdentity(ego, "Cover Identity");

        Assert.Single(ego.Identities);
        Assert.Equal("Cover Identity", identity.Alias);
    }

    [Fact]
    public void RemoveIdentity_RemovesIdentityFromEgo()
    {
        var ego = new Ego();
        _manager.AddIdentity(ego, "First");
        var second = _manager.AddIdentity(ego, "Second");

        var result = _manager.RemoveIdentity(ego, second);

        Assert.True(result);
        Assert.Single(ego.Identities);
    }

    [Fact]
    public void RemoveIdentity_ReturnsFalseForLastIdentity()
    {
        var ego = new Ego();
        var identity = _manager.AddIdentity(ego);

        var result = _manager.RemoveIdentity(ego, identity);

        Assert.False(result);
        Assert.Single(ego.Identities);
    }

    #endregion

    #region Ego Traits Tests

    [Fact]
    public void AddEgoTrait_AddsTraitToEgo()
    {
        var ego = new Ego();

        var trait = _manager.AddEgoTrait(ego, "Common Sense");

        Assert.Single(ego.EgoTraits);
        Assert.Equal("Common Sense", trait.Name);
    }

    [Fact]
    public void RemoveEgoTrait_RemovesTraitFromEgo()
    {
        var ego = new Ego();
        var trait = _manager.AddEgoTrait(ego);

        var result = _manager.RemoveEgoTrait(ego, trait);

        Assert.True(result);
        Assert.Empty(ego.EgoTraits);
    }

    #endregion

    #region Calculation Tests

    [Fact]
    public void CalculateSkillTotal_ReturnsRankPlusAptitude()
    {
        var ego = new Ego
        {
            Aptitudes = new List<EgoAptitude>
            {
                new() { Name = "Somatics", AptitudeValue = 15 }
            },
            Skills = new List<EgoSkill>
            {
                new() { Name = "Athletics", Rank = 40, Aptitude = "Somatics" }
            }
        };

        var total = _manager.CalculateSkillTotal(ego, ego.Skills[0]);

        Assert.Equal(55, total);
    }

    [Fact]
    public void GetTotalPools_CombinesEgoAndMorphPools()
    {
        var ego = new Ego
        {
            EgoFlex = 1,
            Morphs = new List<Morph>
            {
                new()
                {
                    ActiveMorph = true,
                    Insight = 2,
                    Moxie = 1,
                    Vigor = 3,
                    MorphFlex = 1
                }
            }
        };

        var pools = _manager.GetTotalPools(ego);

        Assert.Equal(2, pools.Insight);
        Assert.Equal(1, pools.Moxie);
        Assert.Equal(3, pools.Vigor);
        Assert.Equal(2, pools.Flex); // 1 ego + 1 morph
    }

    [Fact]
    public void GetTotalPools_ReturnsEgoFlexOnlyWhenNoActiveMorph()
    {
        var ego = new Ego { EgoFlex = 2 };

        var pools = _manager.GetTotalPools(ego);

        Assert.Equal(0, pools.Insight);
        Assert.Equal(0, pools.Moxie);
        Assert.Equal(0, pools.Vigor);
        Assert.Equal(2, pools.Flex);
    }

    #endregion

    #region Static Properties Tests

    [Fact]
    public void KnowledgeSkillAptitudes_ContainsOnlyCogAndInt()
    {
        Assert.Equal(2, EgoManager.KnowledgeSkillAptitudes.Count);
        Assert.Contains("Cognition", EgoManager.KnowledgeSkillAptitudes);
        Assert.Contains("Intuition", EgoManager.KnowledgeSkillAptitudes);
    }

    [Fact]
    public void AllAptitudes_ContainsAllSix()
    {
        Assert.Equal(6, EgoManager.AllAptitudes.Count);
        Assert.Contains("Cognition", EgoManager.AllAptitudes);
        Assert.Contains("Intuition", EgoManager.AllAptitudes);
        Assert.Contains("Reflexes", EgoManager.AllAptitudes);
        Assert.Contains("Savvy", EgoManager.AllAptitudes);
        Assert.Contains("Somatics", EgoManager.AllAptitudes);
        Assert.Contains("Willpower", EgoManager.AllAptitudes);
    }

    #endregion
}
