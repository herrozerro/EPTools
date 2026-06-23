using EPTools.Core.Constants;
using EPTools.Core.Interfaces;
using EPTools.Core.Models;
using EPTools.Core.Models.Data;
using EPTools.Core.Models.Ego;
using EPTools.Core.Models.LifePathGen;
using EPTools.Core.Services;

namespace EPTools.Core.Tests;

// ---------------------------------------------------------------------------
// Stubs
// ---------------------------------------------------------------------------

/// <summary>
/// Returns a minimal ego with the six standard aptitudes and one identity.
/// No calls to IEpDataService needed.
/// </summary>
file class StubEgoService : IEgoService
{
    public Task<Ego> GetDefaults()
    {
        var ego = new Ego
        {
            Aptitudes =
            [
                new() { Name = AptitudeNames.Cognition,  ShortName = AptitudeCodes.Cognition,  AptitudeValue = 10 },
                new() { Name = AptitudeNames.Intuition,  ShortName = AptitudeCodes.Intuition,  AptitudeValue = 10 },
                new() { Name = AptitudeNames.Reflexes,   ShortName = AptitudeCodes.Reflexes,   AptitudeValue = 10 },
                new() { Name = AptitudeNames.Savvy,      ShortName = AptitudeCodes.Savvy,      AptitudeValue = 10 },
                new() { Name = AptitudeNames.Somatics,   ShortName = AptitudeCodes.Somatics,   AptitudeValue = 10 },
                new() { Name = AptitudeNames.Willpower,  ShortName = AptitudeCodes.Willpower,  AptitudeValue = 10 },
            ],
            Identities = [ new() { Alias = "Primary" } ]
        };
        return Task.FromResult(ego);
    }
}

/// <summary>
/// Configurable stub for IEpDataService. Only the methods used by LifepathService
/// and EgoService are implemented; all others return empty lists.
/// </summary>
file class StubEpDataService : IEpDataService
{
    public List<Skill> Skills { get; init; } = [];
    public List<Trait> Traits { get; init; } = [];
    public List<MorphTemplate> Morphs { get; init; } = [];
    public Dictionary<string, List<LifePathNode>> Tables { get; init; } = new();

    public Task<List<Skill>> GetSkillsAsync() => Task.FromResult(Skills);
    public Task<List<Trait>> GetTraitsAsync() => Task.FromResult(Traits);
    public Task<List<MorphTemplate>> GetMorphsAsync() => Task.FromResult(Morphs);
    public Task<List<MorphTemplate>> GetAllMorphsAsync() => Task.FromResult(Morphs);
    public Task<List<Trait>> GetAllTraitsAsync() => Task.FromResult(Traits);

    public Task<List<LifePathNode>> GetCharacterGenTableAsync(string tableName) =>
        Task.FromResult(Tables.TryGetValue(tableName, out var nodes) ? nodes : []);

    // Unused by LifepathService — return empty lists
    public Task<List<Aptitude>> GetAptitudesAsync() => Task.FromResult(new List<Aptitude>());
    public Task<List<AptitudeTemplate>> GetAptitudeTemplates() => Task.FromResult(new List<AptitudeTemplate>());
    public Task<List<Background>> GetBackgrounds() => Task.FromResult(new List<Background>());
    public Task<List<Career>> GetCareers() => Task.FromResult(new List<Career>());
    public Task<List<CharGen>> GetCharGen() => Task.FromResult(new List<CharGen>());
    public Task<List<Faction>> GetFactions() => Task.FromResult(new List<Faction>());
    public Task<List<GearArmor>> GetGearArmorsAsync() => Task.FromResult(new List<GearArmor>());
    public Task<List<GearBot>> GetGearBotsAsync() => Task.FromResult(new List<GearBot>());
    public Task<List<GearCategories>> GetGearCategoriesAsync() => Task.FromResult(new List<GearCategories>());
    public Task<List<GearComms>> GetGearCommsAsync() => Task.FromResult(new List<GearComms>());
    public Task<List<GearCreature>> GetGearCreaturesAsync() => Task.FromResult(new List<GearCreature>());
    public Task<List<GearDrug>> GetGearDrugsAsync() => Task.FromResult(new List<GearDrug>());
    public Task<List<GearItem>> GetGearItemsAsync() => Task.FromResult(new List<GearItem>());
    public Task<List<GearMission>> GetGearMissionAsync() => Task.FromResult(new List<GearMission>());
    public Task<List<GearNano>> GetGearNanoAsync() => Task.FromResult(new List<GearNano>());
    public Task<List<GearPack>> GetGearPacksAsync() => Task.FromResult(new List<GearPack>());
    public Task<List<GearSecurity>> GetGearSecurityAsync() => Task.FromResult(new List<GearSecurity>());
    public Task<List<GearService>> GetGearServicesAsync() => Task.FromResult(new List<GearService>());
    public Task<List<GearSoftware>> GetGearSoftwareAsync() => Task.FromResult(new List<GearSoftware>());
    public Task<List<GearSwarm>> GetGearSwarmsAsync() => Task.FromResult(new List<GearSwarm>());
    public Task<List<GearVehicle>> GetGearVehiclesAsync() => Task.FromResult(new List<GearVehicle>());
    public Task<List<GearWare>> GetGearWareAsync() => Task.FromResult(new List<GearWare>());
    public Task<List<GearWeaponAmmo>> GetGearWeaponAmmoAsync() => Task.FromResult(new List<GearWeaponAmmo>());
    public Task<List<GearWeaponMelee>> GetGearWeaponMeleeAsync() => Task.FromResult(new List<GearWeaponMelee>());
    public Task<List<GearWeaponRanged>> GetGearWeaponRangedAsync() => Task.FromResult(new List<GearWeaponRanged>());
    public Task<List<Sleight>> GetSleightsAsync() => Task.FromResult(new List<Sleight>());
    public Task<List<Sleight>> GetAllSleightsAsync() => Task.FromResult(new List<Sleight>());
    public Task<List<Gear>> GetAllGearAsync() => Task.FromResult(new List<Gear>());
    public Task<List<LifePathNode>> GetLifepathNativeTonguesAsync() => Task.FromResult(new List<LifePathNode>());
    public Task<List<LifePathNode>> GetLifepathBackgroundPathAsync() => Task.FromResult(new List<LifePathNode>());
    public Task<List<LifePathNode>> GetLifepathYouthEvents() => Task.FromResult(new List<LifePathNode>());
    public Task<List<LifePathNode>> GetLifepathAges() => Task.FromResult(new List<LifePathNode>());
    public Task<List<LifePathNode>> GetLifepathAdvancedAges() => Task.FromResult(new List<LifePathNode>());
    public Task AddCustomTemplateAsync<T>(T item) where T : EpModel => Task.CompletedTask;
    public Task RemoveCustomTemplateAsync<T>(T item) where T : EpModel => Task.CompletedTask;
    public Task<List<T>> GetCustomTemplatesAsync<T>() => Task.FromResult(new List<T>());
}

/// <summary>Always returns the specified value (clamped to range).</summary>
file class FixedRandomizer(int value = 0) : IRandomizer
{
    public int Next(int minValue, int maxValue) => Math.Clamp(value, minValue, maxValue - 1);
}

// ---------------------------------------------------------------------------
// Tests
// ---------------------------------------------------------------------------

public class LifepathServiceTests
{
    // Convenience factory for LifePathNode
    private static LifePathNode N(string name, LifePathNodeType type, int value = 0, int weight = 0,
        List<List<LifePathNode>>? optionLists = null)
        => new(name, type, "", value, weight, optionLists ?? []);

    // Creates a service with a single-step table
    private static LifepathService Make(
        List<LifePathNode> steps,
        IRandomizer? randomizer = null,
        List<Skill>? skills = null,
        List<Trait>? traits = null,
        List<MorphTemplate>? morphs = null,
        Dictionary<string, List<LifePathNode>>? extraTables = null)
    {
        var tables = new Dictionary<string, List<LifePathNode>>
        {
            ["LifePathSteps"] = steps
        };
        if (extraTables != null)
            foreach (var kv in extraTables)
                tables[kv.Key] = kv.Value;

        var dataService = new StubEpDataService
        {
            Tables = tables,
            Skills = skills ?? [],
            Traits = traits ?? [],
            Morphs = morphs ?? [],
        };

        return new LifepathService(dataService, new StubEgoService(), randomizer ?? new FixedRandomizer(), new EgoManager());
    }

    // --- Simple field assignments ---

    [Fact]
    public async Task Background_SetsEgoBackground()
    {
        var svc = Make([N("Colonist", LifePathNodeType.BackgroundOption)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal("Colonist", ego.Background);
    }

    [Fact]
    public async Task Faction_SetsFaction()
    {
        var svc = Make([N("Anarchist", LifePathNodeType.Faction)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal("Anarchist", ego.Faction);
    }

    [Fact]
    public async Task Interest_SetsInterest()
    {
        var svc = Make([N("Academics", LifePathNodeType.Interest)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal("Academics", ego.Interest);
    }

    [Fact]
    public async Task Career_SetsCareer()
    {
        var svc = Make([N("Scientist", LifePathNodeType.Career)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal("Scientist", ego.Career);
    }

    [Fact]
    public async Task Motivation_AddsToMotivations()
    {
        var svc = Make([N("+Research", LifePathNodeType.Motivation), N("+Freedom", LifePathNodeType.Motivation)]);
        var ego = await svc.GenerateEgo();
        Assert.Contains("+Research", ego.Motivations);
        Assert.Contains("+Freedom", ego.Motivations);
    }

    [Fact]
    public async Task Language_AddsToLanguages()
    {
        var svc = Make([N("English", LifePathNodeType.Language), N("Mandarin", LifePathNodeType.Language)]);
        var ego = await svc.GenerateEgo();
        Assert.Contains("English", ego.Languages);
        Assert.Contains("Mandarin", ego.Languages);
    }

    [Fact]
    public async Task PlayerChoice_AddsToChoices()
    {
        var svc = Make([N("Choose a language", LifePathNodeType.PlayerChoice)]);
        var ego = await svc.GenerateEgo();
        Assert.Contains("Choose a language", ego.PlayerChoices);
    }

    [Fact]
    public async Task StoryEvent_AppendsToNotes()
    {
        var svc = Make([N("You survived a devastating attack", LifePathNodeType.LifePathStoryEvent)]);
        var ego = await svc.GenerateEgo();
        Assert.Contains("You survived a devastating attack", ego.Notes);
    }

    // --- Age ---

    [Fact]
    public async Task Age_SetsEgoAge_WithRandomOffset()
    {
        // Value=20, randomizer always returns 5 → EgoAge = 20 + 5 = 25
        var svc = Make([N("Age", LifePathNodeType.Age, value: 20)], randomizer: new FixedRandomizer(5));
        var ego = await svc.GenerateEgo();
        Assert.Equal(25, ego.EgoAge);
    }

    // --- Aptitude ---

    [Fact]
    public async Task Aptitude_KnownCode_IncreasesAptitudeValue()
    {
        var svc = Make([N("COG", LifePathNodeType.Aptitude, value: 5)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal(15, ego.Aptitudes.First(a => a.Name == AptitudeNames.Cognition).AptitudeValue);
    }

    [Fact]
    public async Task Aptitude_UnknownCode_ChangesNothing()
    {
        var svc = Make([N("XYZ", LifePathNodeType.Aptitude, value: 5)]);
        var ego = await svc.GenerateEgo();
        Assert.All(ego.Aptitudes, a => Assert.Equal(10, a.AptitudeValue));
    }

    // --- Pool ---

    [Fact]
    public async Task Pool_Flex_IncreasesEgoFlex()
    {
        var svc = Make([N("Flex", LifePathNodeType.Pool, value: 2)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal(2, ego.EgoFlex);
    }

    // --- Attribute ---

    [Fact]
    public async Task Attribute_Stress_IncreasesStress()
    {
        var svc = Make([N("Stress", LifePathNodeType.Attribute, value: 3)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal(3, ego.Stress);
    }

    // --- Skills ---

    [Fact]
    public async Task Skill_NewSkill_AddedToEgo()
    {
        var skillTemplate = new Skill("Guns", "REF", Active: true, Combat: true, Physical: false,
            Technical: false, Social: false, Know: false, Field: false, Mental: false, Psi: false,
            Vehicle: false, Description: "", SampleFields: [], Specializations: [], Resource: "", Reference: "", AdditionalRules: []);

        var svc = Make([N("Guns", LifePathNodeType.Skill, value: 40)], skills: [skillTemplate]);
        var ego = await svc.GenerateEgo();

        var skill = ego.Skills.FirstOrDefault(s => s.Name == "Guns");
        Assert.NotNull(skill);
        Assert.Equal(40, skill.Rank);
        Assert.Equal("REF", skill.Aptitude);
        Assert.Equal(SkillType.EgoSkill, skill.SkillType);
    }

    [Fact]
    public async Task Skill_ExistingSkill_StacksRank()
    {
        var skillTemplate = new Skill("Guns", "REF", Active: true, Combat: true, Physical: false,
            Technical: false, Social: false, Know: false, Field: false, Mental: false, Psi: false,
            Vehicle: false, Description: "", SampleFields: [], Specializations: [], Resource: "", Reference: "", AdditionalRules: []);

        // Two Guns nodes — second should stack onto the first
        var svc = Make(
            [N("Guns", LifePathNodeType.Skill, value: 20), N("Guns", LifePathNodeType.Skill, value: 10)],
            skills: [skillTemplate]);
        var ego = await svc.GenerateEgo();

        Assert.Equal(1, ego.Skills.Count(s => s.Name == "Guns"));
        Assert.Equal(30, ego.Skills.First(s => s.Name == "Guns").Rank);
    }

    [Fact]
    public async Task Skill_KnowFlag_SetsKnowledgeSkillType()
    {
        var knowSkill = new Skill("History", "COG", Active: false, Combat: false, Physical: false,
            Technical: false, Social: false, Know: true, Field: false, Mental: false, Psi: false,
            Vehicle: false, Description: "", SampleFields: [], Specializations: [], Resource: "", Reference: "", AdditionalRules: []);

        var svc = Make([N("History", LifePathNodeType.Skill, value: 30)], skills: [knowSkill]);
        var ego = await svc.GenerateEgo();

        Assert.Equal(SkillType.KnowledgeSkill, ego.Skills.First(s => s.Name == "History").SkillType);
    }

    [Fact]
    public async Task Skill_ExoticName_SetsExoticSkillType()
    {
        // Lookup strips the colon-subtype, so the base template name must be "Exotic Skill"
        var exoticSkill = new Skill("Exotic Skill", "REF", Active: true, Combat: true, Physical: false,
            Technical: false, Social: false, Know: false, Field: false, Mental: false, Psi: false,
            Vehicle: false, Description: "", SampleFields: [], Specializations: [], Resource: "", Reference: "", AdditionalRules: []);

        var svc = Make([N("Exotic Skill: Whip", LifePathNodeType.Skill, value: 30)], skills: [exoticSkill]);
        var ego = await svc.GenerateEgo();

        Assert.Equal(SkillType.ExoticSkill, ego.Skills.First(s => s.Name == "Exotic Skill: Whip").SkillType);
    }

    // --- Reputation ---

    [Fact]
    public async Task Reputation_NoIdentity_DoesNotThrow()
    {
        // StubEgoService provides an identity, so remove it via a no-identity ego service
        var dataService = new StubEpDataService
        {
            Tables = new() { ["LifePathSteps"] = [N("ARep", LifePathNodeType.Reputation, value: 10)] }
        };
        // Use a stub ego service that returns an ego with NO identities
        var svc = new LifepathService(dataService, new NoIdentityEgoService(), new FixedRandomizer(), new EgoManager());
        var ego = await svc.GenerateEgo(); // Should not throw
        Assert.Empty(ego.Identities);
    }

    [Fact]
    public async Task Reputation_AddsToFirstIdentity()
    {
        var svc = Make([N("ARep", LifePathNodeType.Reputation, value: 10)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal(10, ego.Identities[0].ARep.Score);
    }

    // --- Sleights ---

    [Fact]
    public async Task Sleight_AddsNSleights()
    {
        var svc = Make([N("Sleight", LifePathNodeType.Sleight, value: 3)]);
        var ego = await svc.GenerateEgo();
        Assert.Equal(3, ego.Psi.Sleights.Count);
    }

    // --- Skip logic ---

    [Fact]
    public async Task Skip_RegistersSection_AndCharacterGenStep_IsSkipped()
    {
        // Skip{Value=1} → SkipSections=[1]
        // CharacterGenStep{Value=1} → skipped, so its child Background is never processed
        var charGenStep = N("Step", LifePathNodeType.CharacterGenStep, value: 1,
            optionLists: [[N("ShouldNotAppear", LifePathNodeType.BackgroundOption)]]);

        var svc = Make([N("Skip", LifePathNodeType.Skip, value: 1), charGenStep]);
        var ego = await svc.GenerateEgo();

        Assert.NotEqual("ShouldNotAppear", ego.Background);
    }

    [Fact]
    public async Task CharacterGenStep_NotInSkipSections_IsProcessed()
    {
        var charGenStep = N("Step", LifePathNodeType.CharacterGenStep, value: 99,
            optionLists: [[N("Anarchist", LifePathNodeType.Faction)]]);

        var svc = Make([charGenStep]);
        var ego = await svc.GenerateEgo();

        Assert.Equal("Anarchist", ego.Faction);
    }

    // --- Option list expansion ---

    [Fact]
    public async Task OptionList_Unweighted_PushesAllItems()
    {
        // Single option list with two unweighted Language nodes → both pushed
        var parent = N("Root", LifePathNodeType.CharacterGenStep, optionLists:
        [
            [
                N("English",  LifePathNodeType.Language, weight: 0),
                N("Mandarin", LifePathNodeType.Language, weight: 0)
            ]
        ]);

        var svc = Make([parent]);
        var ego = await svc.GenerateEgo();

        Assert.Contains("English", ego.Languages);
        Assert.Contains("Mandarin", ego.Languages);
    }

    [Fact]
    public async Task OptionList_Weighted_PicksExactlyOneItem()
    {
        // Two weighted Language options — FixedRandomizer(0) always picks the first
        var parent = N("Root", LifePathNodeType.CharacterGenStep, optionLists:
        [
            [
                N("English",  LifePathNodeType.Language, weight: 1),
                N("Mandarin", LifePathNodeType.Language, weight: 1)
            ]
        ]);

        var svc = Make([parent], randomizer: new FixedRandomizer(0));
        var ego = await svc.GenerateEgo();

        Assert.Single(ego.Languages);
        Assert.Contains("English", ego.Languages);
    }

    // --- Table dispatch ---

    [Fact]
    public async Task Table_PushesSelectedResultOntoStack()
    {
        // Table node points to "TestTable"; TestTable returns a single Career node
        var tableNode = N("TestTable", LifePathNodeType.Table, weight: 0);
        var careerNode = N("Engineer", LifePathNodeType.Career, weight: 1);

        var svc = Make([tableNode],
            randomizer: new FixedRandomizer(0),
            extraTables: new() { ["TestTable"] = [careerNode] });

        var ego = await svc.GenerateEgo();
        Assert.Equal("Engineer", ego.Career);
    }

    // --- Morph ---

    [Fact]
    public async Task Morph_FoundInCatalog_IsAppliedToEgo()
    {
        var template = new MorphTemplate { Name = "Splicer", Type = "Biomorph", Durability = 30 };

        var svc = Make([N("Splicer", LifePathNodeType.Morph)], morphs: [template]);
        var ego = await svc.GenerateEgo();

        Assert.Single(ego.Morphs);
        Assert.Equal("Splicer", ego.Morphs[0].Name);
    }

    [Fact]
    public async Task Morph_NotFoundInCatalog_DoesNotAddMorph()
    {
        var svc = Make([N("Ghost", LifePathNodeType.Morph)], morphs: []);
        var ego = await svc.GenerateEgo();
        Assert.Empty(ego.Morphs);
    }

    // --- Trait ---

    [Fact]
    public async Task Trait_FoundInCatalog_IsAddedToEgoTraits()
    {
        var traitTemplate = new Trait { Name = "Brave", Type = "Positive", Summary = "Fear less.", Cost = [5] };

        var svc = Make([N("Brave", LifePathNodeType.Trait, value: 1)], traits: [traitTemplate]);
        var ego = await svc.GenerateEgo();

        var trait = ego.EgoTraits.FirstOrDefault(t => t.Name == "Brave");
        Assert.NotNull(trait);
        Assert.Equal("Positive", trait.Type);
    }

    // --- ForcedInterest ---

    [Fact]
    public async Task ForcedInterest_FoundInInterestTable_PushesInterestNode()
    {
        var interestNode = N("Art", LifePathNodeType.Interest);

        var svc = Make(
            [N("Art", LifePathNodeType.ForcedInterest)],
            extraTables: new() { ["LifePathInterest"] = [interestNode] });

        var ego = await svc.GenerateEgo();
        Assert.Equal("Art", ego.Interest);
    }

    [Fact]
    public async Task ForcedInterest_NotFound_LogsToOutput()
    {
        var svc = Make([N("MissingInterest", LifePathNodeType.ForcedInterest)],
            extraTables: new() { ["LifePathInterest"] = [] });

        var ego = await svc.GenerateEgo();
        // Should not throw; output should contain an error note
        Assert.Contains(ego.CharacterGenerationOutput, o => o.Contains("did not load properly"));
    }

    // --- Output ---

    [Fact]
    public async Task GenerateEgo_PopulatesCharacterGenerationOutput()
    {
        var svc = Make([N("Colonist", LifePathNodeType.BackgroundOption)]);
        var ego = await svc.GenerateEgo();
        Assert.NotEmpty(ego.CharacterGenerationOutput);
    }
}

// Minimal ego service with no identities (for reputation guard test)
file class NoIdentityEgoService : IEgoService
{
    public Task<Ego> GetDefaults() => Task.FromResult(new Ego());
}
