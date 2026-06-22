using EPTools.Core.Extensions;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;
using EPTools.Core.Models.LifePathGen;
using EPTools.Core.Constants;

namespace EPTools.Core.Services;

public class LifepathService : ILifepathService
{
    private readonly Dictionary<LifePathNodeType, Func<LifepathContext, LifePathNode, Task>> _applyNodeMethods = new();
    private readonly IEpDataService _ePDataService;
    private readonly IEgoService _egoService;
    private readonly IRandomizer _randomizer;
    private readonly EgoManager _egoManager;

    public LifepathService(IEpDataService ePDataService, IEgoService egoService, IRandomizer randomizer, EgoManager egoManager)
    {
        _ePDataService = ePDataService;
        _egoService = egoService;
        _randomizer = randomizer;
        _egoManager = egoManager;
        InitializeApplyMethods();
    }

    private void InitializeApplyMethods()
    {
        _applyNodeMethods[LifePathNodeType.Morph] = ApplyMorph;
        _applyNodeMethods[LifePathNodeType.Skill] = ApplySkill;
        _applyNodeMethods[LifePathNodeType.Trait] = ApplyTrait;
        _applyNodeMethods[LifePathNodeType.Aptitude] = ApplyAptitude;
        _applyNodeMethods[LifePathNodeType.ForcedInterest] = ApplyForcedInterest;
        _applyNodeMethods[LifePathNodeType.Language] = ApplyLanguage;
        _applyNodeMethods[LifePathNodeType.Pool] = ApplyPool;
        _applyNodeMethods[LifePathNodeType.Reputation] = ApplyReputation;
        _applyNodeMethods[LifePathNodeType.Sleight] = ApplySleight;
        _applyNodeMethods[LifePathNodeType.Faction] = ApplyFaction;
        _applyNodeMethods[LifePathNodeType.Age] = ApplyAge;
        _applyNodeMethods[LifePathNodeType.Motivation] = ApplyMotivation;
        _applyNodeMethods[LifePathNodeType.Skip] = ApplySkip;
        _applyNodeMethods[LifePathNodeType.PlayerChoice] = ApplyPlayerChoice;
        _applyNodeMethods[LifePathNodeType.Interest] = ApplyInterest;
        _applyNodeMethods[LifePathNodeType.Career] = ApplyCareer;
        _applyNodeMethods[LifePathNodeType.BackgroundOption] = ApplyBackground;
        _applyNodeMethods[LifePathNodeType.Table] = ApplyTable;
        _applyNodeMethods[LifePathNodeType.LifePathStoryEvent] = ApplyLifePathStoryEvent;
        _applyNodeMethods[LifePathNodeType.Attribute] = ApplyAttribute;
    }

    // Expands each option list independently: weighted lists pick one, unweighted push all.
    private IEnumerable<LifePathNode> ExpandOptionLists(IEnumerable<List<LifePathNode>> optionLists)
    {
        foreach (var list in optionLists)
        {
            if (list.Sum(x => x.Weight) > 0)
                yield return list.GetWeightedItem(_randomizer);
            else
                foreach (var item in list)
                    yield return item;
        }
    }

    // Pushes nodes in reverse so the first item is processed first (LIFO).
    private void PushAll(LifepathContext ctx, IEnumerable<LifePathNode> nodes)
    {
        foreach (var node in nodes.Reverse())
            ctx.Nodes.Push(node);
    }

    public async Task<Ego> GenerateEgo()
    {
        var ego = await _egoService.GetDefaults();
        var ctx = new LifepathContext(ego);

        var charGenSteps = await _ePDataService.GetCharacterGenTableAsync("LifePathSteps");
        PushAll(ctx, charGenSteps);

        while (ctx.Nodes.Any())
        {
            var node = ctx.Nodes.Pop();
            await DispatchNode(ctx, node);
        }

        ego.CharacterGenerationOutput = ctx.Output;
        ego.PlayerChoices = ctx.Choices;

        return ego;
    }

    private async Task DispatchNode(LifepathContext ctx, LifePathNode node)
    {
        ctx.Output.Add($"{node.Name} {node.Description}".Trim());

        // Skip logic lives here, not on the node — avoids mutating record data.
        if (node.Type == LifePathNodeType.CharacterGenStep && ctx.SkipSections.Contains(node.Value))
            return;

        if (_applyNodeMethods.TryGetValue(node.Type, out var apply))
            await apply(ctx, node);

        PushAll(ctx, ExpandOptionLists(node.OptionLists));
    }

    private Task ApplyPlayerChoice(LifepathContext ctx, LifePathNode option)
    {
        ctx.Choices.Add(option.Name);
        return Task.CompletedTask;
    }

    private Task ApplyBackground(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Background = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplySkip(LifepathContext ctx, LifePathNode option)
    {
        ctx.SkipSections.Add(option.Value);
        return Task.CompletedTask;
    }

    private Task ApplyMotivation(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Motivations.Add(option.Name);
        return Task.CompletedTask;
    }

    private Task ApplyAge(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.EgoAge = option.Value + _randomizer.Next(0, 9);
        return Task.CompletedTask;
    }

    private async Task ApplyForcedInterest(LifepathContext ctx, LifePathNode option)
    {
        var selectedInterest = (await _ePDataService.GetCharacterGenTableAsync("LifePathInterest")).FirstOrDefault(x => x.Name == option.Name);
        if (selectedInterest != null)
        {
            ctx.Nodes.Push(selectedInterest);
            return;
        }
        ctx.Output.Add($"{option.Name} {option.Description} did not load properly".Trim());
    }

    private Task ApplyInterest(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Interest = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplyFaction(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Faction = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplySleight(LifepathContext ctx, LifePathNode option)
    {
        for (var i = 0; i < option.Value; i++)
        {
            _egoManager.AddSleight(ctx.Ego, new EgoSleight
            {
                Name = "Random or chosen",
                Description = string.Empty,
                Level = string.Empty,
                Duration = string.Empty,
                Action = string.Empty,
                Summary = string.Empty
            });
        }
        return Task.CompletedTask;
    }

    private async Task ApplyTrait(LifepathContext ctx, LifePathNode option)
    {
        var trait = (await _ePDataService.GetTraitsAsync()).FirstOrDefault(x => x.Name == option.Name.Split("-")[0]);
        _egoManager.AddEgoTrait(ctx.Ego, new EgoTrait
        {
            Name = option.Name,
            Description = trait?.Description ?? "",
            Level = option.Value,
            CostTiers = string.Join(",", trait?.Cost ?? []),
            Cost = trait?.Cost[Math.Clamp(option.Value - 1, 0, trait.Cost.Count - 1)] ?? 0,
            Type = trait?.Type ?? "",
            Summary = trait?.Summary ?? "",
            AdditionalRules = trait?.AdditionalRules ?? []
        });
    }

    private Task ApplyAptitude(LifepathContext ctx, LifePathNode option)
    {
        var aptitudeToChange = option.Name.ToUpper() switch
        {
            AptitudeCodes.Cognition => AptitudeNames.Cognition,
            AptitudeCodes.Intuition => AptitudeNames.Intuition,
            AptitudeCodes.Reflexes => AptitudeNames.Reflexes,
            AptitudeCodes.Savvy => AptitudeNames.Savvy,
            AptitudeCodes.Somatics => AptitudeNames.Somatics,
            AptitudeCodes.Willpower => AptitudeNames.Willpower,
            _ => string.Empty
        };

        var existingAptitude = ctx.Ego.Aptitudes.FirstOrDefault(x => x.Name == aptitudeToChange);
        if (existingAptitude != null)
            existingAptitude.AptitudeValue += option.Value;

        return Task.CompletedTask;
    }

    private Task ApplyReputation(LifepathContext ctx, LifePathNode option)
    {
        if (ctx.Ego.Identities.Count == 0)
            return Task.CompletedTask;

        _egoManager.AddReputation(ctx.Ego.Identities[0], option.Name, option.Value);
        return Task.CompletedTask;
    }

    private Task ApplyPool(LifepathContext ctx, LifePathNode option)
    {
        switch (option.Name)
        {
            case "Flex":
                ctx.Ego.EgoFlex += option.Value;
                break;
        }
        return Task.CompletedTask;
    }

    private Task ApplyLanguage(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Languages.Add(option.Name);
        return Task.CompletedTask;
    }

    private async Task ApplySkill(LifepathContext ctx, LifePathNode option)
    {
        var skills = await _ePDataService.GetSkillsAsync();
        var parts = option.Name.Split("-");
        var skillName = parts[0];
        var specialization = parts.Length > 1 ? parts[1] : "";
        var template = skills.FirstOrDefault(x => x.Name == skillName.Split(":")[0].Trim());
        var skillType = template switch
        {
            { Know: true } => SkillType.KnowledgeSkill,
            { Name: var n } when n.StartsWith("Exotic Skill", StringComparison.OrdinalIgnoreCase) => SkillType.ExoticSkill,
            _ => SkillType.EgoSkill
        };

        if (ctx.Ego.Skills.Any(x => x.Name == skillName))
        {
            var existing = ctx.Ego.Skills.First(x => x.Name == skillName);
            existing.Rank += option.Value;
            existing.Specialization += specialization;
        }
        else
        {
            ctx.Ego.Skills.Add(new EgoSkill
            {
                Name = skillName,
                Rank = option.Value,
                Specialization = specialization,
                Aptitude = template?.Aptitude ?? "",
                SkillType = skillType
            });
        }
    }

    private async Task ApplyMorph(LifepathContext ctx, LifePathNode option)
    {
        var template = (await _ePDataService.GetMorphsAsync()).FirstOrDefault(x => x.Name == option.Name);
        if (template != null)
            _egoManager.ApplyMorphTemplate(ctx.Ego, template);
    }

    private Task ApplyCareer(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Career = option.Name;
        return Task.CompletedTask;
    }

    // Pushes the selected table result back onto the stack so it is dispatched
    // through the normal flow — preserving its Type effect and OptionLists expansion.
    private async Task ApplyTable(LifepathContext ctx, LifePathNode option)
    {
        var result = (await _ePDataService.GetCharacterGenTableAsync(option.Name))
            .GetWeightedItem(_randomizer, option.Value);
        ctx.Nodes.Push(result);
    }

    private Task ApplyLifePathStoryEvent(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Notes += option.Name + Environment.NewLine;
        return Task.CompletedTask;
    }

    private Task ApplyAttribute(LifepathContext ctx, LifePathNode option)
    {
        switch (option.Name)
        {
            case "Stress":
                ctx.Ego.Stress += option.Value;
                break;
        }
        return Task.CompletedTask;
    }
}
