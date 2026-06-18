using EPTools.Core.Extensions;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;
using EPTools.Core.Models.LifePathGen;
using EPTools.Core.Constants;

namespace EPTools.Core.Services;

public class LifepathService
{
    private readonly Dictionary<string, Func<LifepathContext, LifePathNode, Task>> _applyNodeMethods = new();
    private readonly IEpDataService _ePDataService;
    private readonly EgoService _egoService;
    private readonly IRandomizer _randomizer;

    public LifepathService(IEpDataService ePDataService, EgoService egoService, IRandomizer randomizer)
    {
        _ePDataService = ePDataService;
        _egoService = egoService;
        _randomizer = randomizer;
        InitializeApplyMethods();
    }

    private void InitializeApplyMethods()
    {
        _applyNodeMethods["Morph"] = ApplyMorph;
        _applyNodeMethods["Skill"] = ApplySkill;
        _applyNodeMethods["Trait"] = ApplyTrait;
        _applyNodeMethods["Aptitude"] = ApplyAptitude;
        _applyNodeMethods["ForcedInterest"] = ApplyForcedInterest;
        _applyNodeMethods["Language"] = ApplyLanguage;
        _applyNodeMethods["Pool"] = ApplyPool;
        _applyNodeMethods["Reputation"] = ApplyReputation;
        _applyNodeMethods["Sleight"] = ApplySleight;
        _applyNodeMethods["Faction"] = ApplyFaction;
        _applyNodeMethods["Age"] = ApplyAge;
        _applyNodeMethods["Motivation"] = ApplyMotivation;
        _applyNodeMethods["Skip"] = ApplySkip;
        _applyNodeMethods["PlayerChoice"] = ApplyPlayerChoice;
        _applyNodeMethods["Interest"] = ApplyInterest;
        _applyNodeMethods["Career"] = ApplyCareer;
        _applyNodeMethods["BackgroundOption"] = ApplyBackground;
        _applyNodeMethods["Table"] = ApplyTable;
        _applyNodeMethods["LifePathStoryEvent"] = ApplyLifePathStoryEvent;
        _applyNodeMethods["Attribute"] = ApplyAttribute;
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
        if (node.Type == "CharacterGenStep" && ctx.SkipSections.Contains(node.Value))
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
            ctx.Ego.Psi.Sleights.Add(new EgoSleight
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
        ctx.Ego.EgoTraits.Add(new EgoTrait {
            Name = option.Name,
            Description = trait?.Description ?? "",
            Level = option.Value,
            CostTiers = string.Join(",",trait?.Cost ?? []),
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

        var identity = ctx.Ego.Identities[0];
        var repToChange = option.Name switch
        {
            "ARep" => identity.ARep,
            "CRep" => identity.CRep,
            "GRep" => identity.GRep,
            "IRep" => identity.IRep,
            "XRep" => identity.XRep,
            "RRep" => identity.RRep,
            _ => null
        };
        if (repToChange != null)
            repToChange.Score += option.Value;

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
        var skillName = option.Name.Split("-")[0];
        var skillType = skillName switch
        {
            { } s when s.Contains("know", StringComparison.OrdinalIgnoreCase) => SkillType.KnowledgeSkill,
            { } s when s.Contains("exotic", StringComparison.OrdinalIgnoreCase) => SkillType.ExoticSkill,
            _ => SkillType.EgoSkill
        };

        if (ctx.Ego.Skills.Any(x => x.Name == skillName))
        {
            ctx.Ego.Skills.First(x=>x.Name == skillName).Rank += option.Value;
            var specialization = option.Name.Split("-").Length > 1 ? option.Name.Split("-")[1] : "";
            ctx.Ego.Skills.First(x=>x.Name == skillName).Specialization += specialization;
        }
        else
        {
            ctx.Ego.Skills.Add(
                new EgoSkill
                {
                    Name = option.Name.Split("-")[0],
                    Rank = option.Value,
                    Specialization = option.Name.Split("-").Length > 1 ? option.Name.Split("-")[1] : "",
                    Aptitude = skills.FirstOrDefault(x=>x.Name == option.Name.Split("-")[0].Split(":")[0])?.Aptitude ?? "",
                    SkillType = skillType
                });
        }
    }

    private async Task ApplyMorph(LifepathContext ctx, LifePathNode option)
    {
        var selectedMorph = (await _ePDataService.GetMorphsAsync()).FirstOrDefault(x => x.Name == option.Name);
        if (selectedMorph != null)
        {
            ctx.Ego.Morphs.Clear();
            ctx.Ego.Morphs.Add(new Morph
            {
                Name = option.Name,
                ActiveMorph = true,
                Insight = selectedMorph.Pools.Insight,
                Moxie = selectedMorph.Pools.Moxie,
                Vigor = selectedMorph.Pools.Vigor,
                MorphFlex = selectedMorph.Pools.Flex,
                MorphType = selectedMorph.Type,
                MorphSex = "",
                Traits = selectedMorph.MorphTraits.Select(x => new EgoTrait { Name = x.Name, Level = x.Level }).ToList(),
                Wares = selectedMorph.Ware.Select(x=> new Ware { Name = x}).ToList()
            });
        }
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
