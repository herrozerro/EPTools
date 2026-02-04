using EPTools.Core.Extensions;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;
using EPTools.Core.Models.LifePathGen;
using EPTools.Core.Constants;
using Morph = EPTools.Core.Models.Ego.Morph;

namespace EPTools.Core.Services;

public class LifepathService(EpDataService ePDataService, EgoService egoService, IRandomizer randomizer)
{
    private readonly Dictionary<string, Func<LifepathContext, LifePathNode, Task>> _applyNodeMethods = new();

    private void InitializeApplyMethods()
    {
        // Async methods can be added directly
        _applyNodeMethods["Morph"] = ApplyMorph;
        _applyNodeMethods["Skill"] = ApplySkill;
        _applyNodeMethods["Trait"] = ApplyTrait;
        _applyNodeMethods["Aptitude"] = ApplyAptitude;
        _applyNodeMethods["ForcedInterest"] = ApplyForcedInterest;

        // Sync methods refactored to return Task for consistency
        _applyNodeMethods["Language"] = ApplyLanguage;
        _applyNodeMethods["Pool"] = ApplyPool;
        _applyNodeMethods["Reputation"] = ApplyReputation;
        _applyNodeMethods["Slight"] = ApplySlight;
        _applyNodeMethods["Faction"] = ApplyFaction;
        _applyNodeMethods["Age"] = ApplyAge;
        _applyNodeMethods["Motivation"] = ApplyMotivation;
        _applyNodeMethods["Skip"] = ApplySkip;
        _applyNodeMethods["PlayerChoice"] = ApplyPlayerChoice;

        // Logic extracted from inline lambdas to methods
        _applyNodeMethods["Interest"] = ApplyInterest;
        _applyNodeMethods["Career"] = ApplyCareer;
        _applyNodeMethods["BackgroundOption"] = ApplyBackground;
        _applyNodeMethods["CharacterGenStep"] = ApplyCharacterGenStep;
        _applyNodeMethods["Table"] = ApplyTable;
        _applyNodeMethods["LifePathStoryEvent"] = ApplyLifePathStoryEvent;
        _applyNodeMethods["Attribute"] = ApplyAttribute;
    }

    private void ProcessOptionLists(LifepathContext ctx, LifePathNode option)
    {
        if (option.OptionLists.Count == 0) return;

        foreach (var list in option.OptionLists)
        {
            if (list.Sum(x => x.Weight) > 0)
            {
                ctx.Nodes.Push(list.GetWeightedItem(randomizer: randomizer));
                return;
            }

            foreach (var item in list)
            {
                ctx.Nodes.Push(item);
            }
        }
    }

    public async Task<Ego> GenerateEgo()
    {
        var ego = await egoService.GetDefaults();
        var ctx = new LifepathContext(ego);

        var charGenSteps = await ePDataService.GetCharacterGenTableAsync("LifePathSteps");

        charGenSteps.Reverse();

        charGenSteps.ForEach(x => ctx.Nodes.Push(x));

        while (ctx.Nodes.Any())
        {
            var node = ctx.Nodes.Pop();
            Console.WriteLine($"{node.Name} {node.Description} {node.Value}");
            await ApplyBackgroundOption(ctx, node);
        }

        ego.CharacterGenerationOutput = ctx.Output;
        ego.PlayerChoices = ctx.Choices;

        return ego;
    }

    // This method (ApplyBackgroundOption) is the main dispatcher
    private async Task ApplyBackgroundOption(LifepathContext ctx, LifePathNode option)
    {
        if (_applyNodeMethods.Count == 0) InitializeApplyMethods();

        ctx.Output.Add($"{option.Name} {option.Description}".Trim());

        if (_applyNodeMethods.TryGetValue(option.Type, out var applyMethod))
            await applyMethod(ctx, option);
        ProcessOptionLists(ctx, option);
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
        ctx.Ego.EgoAge = option.Value + randomizer.Next(0, 9);
        return Task.CompletedTask;
    }

    private async Task ApplyForcedInterest(LifepathContext ctx, LifePathNode option)
    {
        var selectedInterest = (await ePDataService.GetCharacterGenTableAsync("LifePathInterest")).FirstOrDefault(x => x.Name == option.Name);
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

    private Task ApplySlight(LifepathContext ctx, LifePathNode option)
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
        var trait = (await ePDataService.GetTraitsAsync()).FirstOrDefault(x => x.Name == option.Name.Split("-")[0]);
        ctx.Ego.EgoTraits.Add(new EgoTrait {
            Name = option.Name,
            Description = trait?.Description ?? "",
            Level = option.Value,
            CostTiers = string.Join(",",trait?.Cost ?? []),
            Cost = trait?.Cost[Math.Max(option.Value-1,0)] ?? 0,
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

        //check if aptitude already exists and add value to it
        var existingAptitude = ctx.Ego.Aptitudes.FirstOrDefault(x => x.Name == aptitudeToChange);
        if (existingAptitude != null)
            existingAptitude.AptitudeValue += option.Value;

        return Task.CompletedTask;
    }

    private Task ApplyReputation(LifepathContext ctx, LifePathNode option)
    {
        var repToChange = option.Name switch
        {
            "ARep" => ctx.Ego.Identities[0].ARep,
            "CRep" => ctx.Ego.Identities[0].CRep,
            "GRep" => ctx.Ego.Identities[0].GRep,
            "IRep" => ctx.Ego.Identities[0].IRep,
            "XRep" => ctx.Ego.Identities[0].XRep,
            "RRep" => ctx.Ego.Identities[0].RRep,
            _ => null
        };
        if (repToChange != null)
        {
            repToChange.Score += option.Value;
        }
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
        // add language to languages
        ctx.Ego.Languages += option.Name + ", ";
        return Task.CompletedTask;
    }

    private async Task ApplySkill(LifepathContext ctx, LifePathNode option)
    {
        var skills = await ePDataService.GetSkillsAsync();
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
        var selectedMorph = (await ePDataService.GetMorphsAsync()).FirstOrDefault(x => x.Name == option.Name);
        if (selectedMorph != null)
        {
            ctx.Ego.Morphs.Clear();
            ctx.Ego.Morphs.Add(new Morph
            {
                Name = option.Name,
                ActiveMorph = false,
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

    // New extracted methods for logic previously inside lambdas

    private Task ApplyCareer(LifepathContext ctx, LifePathNode option)
    {
        ctx.Ego.Career = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplyCharacterGenStep(LifepathContext ctx, LifePathNode option)
    {
        if (ctx.SkipSections.Contains(option.Value))
            option.OptionLists.Clear();
        return Task.CompletedTask;
    }

    private async Task ApplyTable(LifepathContext ctx, LifePathNode option)
    {
        var nodes = await ProcessTableRequest(option.Name, option.Value, ctx);
        nodes.ForEach(x => ctx.Nodes.Push(x));
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

    private async Task<List<LifePathNode>> ProcessTableRequest(string tableName, int tableModifier, LifepathContext ctx)
    {
        List<LifePathNode> options = new();

        var table = (await ePDataService.GetCharacterGenTableAsync(tableName)).GetWeightedItem(tableModifier, randomizer);

        ctx.Output.Add($"{table.Name} {table.Description}".Trim());

        if (table.OptionLists.Count == 0)
        {
            return [table];
        }

        foreach (var nodeList in table.OptionLists)
        {
            if (nodeList.Sum(x => x.Weight) > 0)
            {
                options.Add(nodeList.GetWeightedItem(randomizer: randomizer));
            }
            else
            {
                options.AddRange(nodeList);
            }
        }

        return options;
    }
}
