using EPTools.Core.Extensions;
using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;
using EPTools.Core.Models.LifePathGen;
using EPTools.Core.Constants;
using Morph = EPTools.Core.Models.Ego.Morph;

namespace EPTools.Core.Services;

public class LifepathService(EpDataService ePDataService, EgoService egoService, IRandomizer randomizer)
{
    private Ego? NewEgo { get; set; }
        
    private readonly Dictionary<string, Func<Ego, LifePathNode, Task>> _applyNodeMethods = new();

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
                
    private void ProcessOptionLists(Ego ego, LifePathNode option)
    {
        if (option.OptionLists.Count == 0) return;

        foreach (var list in option.OptionLists)
        {
            if (list.Sum(x => x.Weight) > 0)
            {
                ego.CharacterGenerationNodes.Push(list.GetWeightedItem(randomizer: randomizer));
                return;
            }

            foreach (var item in list)
            {
                ego.CharacterGenerationNodes.Push(item);
            }
        }
    }
        
    public async Task<Ego> GenerateEgo()
    {
        NewEgo = await egoService.GetDefaults();

        var charGenSteps = await ePDataService.GetCharacterGenTableAsync("LifePathSteps");

        charGenSteps.Reverse();

        charGenSteps.ForEach(x=> NewEgo.CharacterGenerationNodes.Push(x));

        while (NewEgo.CharacterGenerationNodes.Any())
        {
            var node = NewEgo.CharacterGenerationNodes.Pop();
            Console.WriteLine($"{node.Name} {node.Description} {node.Value}");
            await ApplyBackgroundOption(NewEgo, node);
        }

        return NewEgo;
    }

    // This method (ApplyBackgroundOption) is the main dispatcher
    private async Task ApplyBackgroundOption(Ego ego, LifePathNode option)
    {
        if (_applyNodeMethods.Count == 0) InitializeApplyMethods();

        ego.CharacterGenerationOutput.Add($"{option.Name} {option.Description}".Trim());

        if (_applyNodeMethods.TryGetValue(option.Type, out var applyMethod)) 
            await applyMethod(ego, option);
        ProcessOptionLists(ego, option);
    }

    private Task ApplyPlayerChoice(Ego ego, LifePathNode option)
    {
        ego.PlayerChoices.Add(option.Name);
        return Task.CompletedTask;
    }

    private Task ApplyBackground(Ego ego, LifePathNode option)
    {
        ego.Background = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplySkip(Ego ego, LifePathNode option)
    {
        ego.SkipSections.Add(option.Value);
        return Task.CompletedTask;
    }

    private Task ApplyMotivation(Ego ego, LifePathNode option)
    {
        ego.Motivations.Add(option.Name);
        return Task.CompletedTask;
    }

    private Task ApplyAge(Ego ego, LifePathNode option)
    {
        ego.EgoAge = option.Value + randomizer.Next(0, 9);
        return Task.CompletedTask;
    }

    private async Task ApplyForcedInterest(Ego ego, LifePathNode option)
    {
        var selectedInterest = (await ePDataService.GetCharacterGenTableAsync("LifePathInterest")).FirstOrDefault(x => x.Name == option.Name);
        if (selectedInterest != null)
        {
            ego.CharacterGenerationNodes.Push(selectedInterest);
            return;
        }
        ego.CharacterGenerationOutput.Add($"{option.Name} {option.Description} did not load properly".Trim());
    }

    private Task ApplyInterest(Ego ego, LifePathNode option)
    {
        ego.Interest = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplyFaction(Ego ego, LifePathNode option)
    {
        ego.Faction = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplySlight(Ego ego, LifePathNode option)
    {
        for (var i = 0; i < option.Value; i++)
        {
            ego.Psi.Sleights.Add(new EgoSleight
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

    private async Task ApplyTrait(Ego ego, LifePathNode option)
    {
        var trait = (await ePDataService.GetTraitsAsync()).FirstOrDefault(x => x.Name == option.Name.Split("-")[0]);
        ego.EgoTraits.Add(new EgoTrait { 
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

    private Task ApplyAptitude(Ego ego, LifePathNode option)
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
        var existingAptitude = ego.Aptitudes.FirstOrDefault(x => x.Name == aptitudeToChange);
        if (existingAptitude != null)
            existingAptitude.AptitudeValue += option.Value;
        
        return Task.CompletedTask;
    }

    private Task ApplyReputation(Ego ego, LifePathNode option)
    {
        var repToChange = option.Name switch
        {
            "ARep" => ego.Identities[0].ARep,
            "CRep" => ego.Identities[0].CRep,
            "GRep" => ego.Identities[0].GRep,
            "IRep" => ego.Identities[0].IRep,
            "XRep" => ego.Identities[0].XRep,
            "RRep" => ego.Identities[0].RRep,
            _ => null
        };
        if (repToChange != null)
        {
            repToChange.Score += option.Value;
        }
        return Task.CompletedTask;
    }

    private Task ApplyPool(Ego ego, LifePathNode option)
    {
        switch (option.Name)
        {
            case "Flex":
                ego.EgoFlex += option.Value;
                break;
        }
        return Task.CompletedTask;
    }

    private Task ApplyLanguage(Ego ego, LifePathNode option)
    {
        // add language to languages
        ego.Languages += option.Name + ", ";
        return Task.CompletedTask;
    }

    private async Task ApplySkill(Ego ego, LifePathNode option)
    {
        var skills = await ePDataService.GetSkillsAsync();
        var skillName = option.Name.Split("-")[0];
        var skillType = skillName switch
        {
            { } s when s.Contains("know", StringComparison.OrdinalIgnoreCase) => SkillType.KnowledgeSkill,
            { } s when s.Contains("exotic", StringComparison.OrdinalIgnoreCase) => SkillType.ExoticSkill,
            _ => SkillType.EgoSkill
        };

        if (ego.Skills.Any(x => x.Name == skillName))
        {
            ego.Skills.First(x=>x.Name == skillName).Rank += option.Value;
            var specialization = option.Name.Split("-").Length > 1 ? option.Name.Split("-")[1] : "";
            ego.Skills.First(x=>x.Name == skillName).Specialization += specialization;
        }
        else
        {
            ego.Skills.Add(
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

    private async Task ApplyMorph(Ego ego, LifePathNode option)
    {
        var selectedMorph = (await ePDataService.GetMorphsAsync()).FirstOrDefault(x => x.Name == option.Name);
        if (selectedMorph != null)
        {
            ego.Morphs.Clear();
            ego.Morphs.Add(new Morph
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
        
    private Task ApplyCareer(Ego ego, LifePathNode option)
    {
        ego.Career = option.Name;
        return Task.CompletedTask;
    }

    private Task ApplyCharacterGenStep(Ego ego, LifePathNode option)
    {
        if (ego.SkipSections.Contains(option.Value))
            option.OptionLists.Clear();
        return Task.CompletedTask;
    }

    private async Task ApplyTable(Ego ego, LifePathNode option)
    {
        var nodes = await ProcessTableRequest(option.Name, option.Value, ego);
        nodes.ForEach(x => ego.CharacterGenerationNodes.Push(x));
    }

    private Task ApplyLifePathStoryEvent(Ego ego, LifePathNode option)
    {
        ego.Notes += option.Name + Environment.NewLine;
        return Task.CompletedTask;
    }

    private Task ApplyAttribute(Ego ego, LifePathNode option)
    {
        switch (option.Name)
        {
            case "Stress":
                ego.Stress += option.Value;
                break;
        }
        return Task.CompletedTask;
    }

    private async Task<List<LifePathNode>> ProcessTableRequest(string tableName, int tableModifier, Ego ego)
    {
        List<LifePathNode> options = new();

        var table = (await ePDataService.GetCharacterGenTableAsync(tableName)).GetWeightedItem(tableModifier, randomizer);

        ego.CharacterGenerationOutput.Add($"{table.Name} {table.Description}".Trim());

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