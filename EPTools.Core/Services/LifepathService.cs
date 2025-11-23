using System.Diagnostics;
using EPTools.Core.Extensions;
using EPTools.Core.Models.Ego;
using EPTools.Core.Models.LifePathGen;

namespace EPTools.Core.Services
{
    public class LifepathService(EpDataService ePDataService, EgoService egoService)
    {
        private Ego? NewEgo { get; set; }

        public async Task<Ego> GenerateEgo()
        {
            NewEgo = await egoService.GetDefaults();

            var charGenSteps = await ePDataService.GetCharacterGenTable("LifePathSteps");

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

        private async Task ApplyBackgroundOption(Ego ego, LifePathNode option)
        {
            ego.CharacterGenerationOutput.Add($"{option.Name} {option.Description}".Trim());
            switch (option.Type)
            {
                case "Morph":
                    await ApplyMorph(ego, option);
                    break;
                case "Skill":
                    await ApplySkill(ego, option);
                    break;
                case "Trait":
                    await ApplyTrait(ego, option);
                    break;
                case "Language":
                    ApplyLanguage(ego, option);
                    break;
                case "Pool":
                    ApplyPool(ego, option);
                    break;
                case "Reputation":
                    ApplyReputation(ego, option);
                    break;
                case "Aptitude":
                    await ApplyAptitude(ego, option);
                    break;
                case "Slight":
                    ApplySlight(ego, option);
                    break;
                case "Faction":
                    ApplyFaction(ego, option);
                    break;
                case "Interest":
                    ApplyInterest(ego, option);
                    goto default;
                case "ForcedInterest":
                    await ApplyForcedInterest(ego, option);
                    break;
                case "Career":
                    ego.Career = option.Name;
                    goto default;
                case "Age":
                    ApplyAge(ego, option);
                    break;
                case "Motivation":
                    ApplyMotivation(ego, option);
                    break;
                case "Skip":
                    ApplySkip(ego, option);
                    break;
                case "CharacterGenStep":
                    if (NewEgo.SkipSections.Contains(option.Value))
                    {
                        break;
                    }
                    goto default;
                case "BackgroundOption":
                    ApplyBackground(ego, option);
                    goto default;
                case "Table":
                    var nodes = await ProcessTableRequest(option.Name, option.Value, ego);
                    nodes.ForEach(x=>ego.CharacterGenerationNodes.Push(x));
                    break;
                case "LifePathStoryEvent":
                    ego.Notes += option.Name + Environment.NewLine;
                    break;
                case "Attribute":
                    //TODO: implement the ability to start with stress
                    //throw new NotImplementedException();
                    break;
                case "PlayerChoice":
                    ApplyPlayerChoice(ego, option);
                    break;
                default:
                    foreach (var list in option.OptionLists)
                    {
                        if (list.Sum(x => x.Weight) > 0)
                        {
                            ego.CharacterGenerationNodes.Push(list.GetWeightedItem());
                        }
                        else
                        {
                            foreach (var item in list)
                            {
                                ego.CharacterGenerationNodes.Push(item);
                            }
                        }
                    }
                    break;
            }
        }

        private void ApplyPlayerChoice(Ego ego, LifePathNode option)
        {
            ego.PlayerChoices.Add(option.Name);
        }

        private void ApplyBackground(Ego ego, LifePathNode option)
        {
            ego.Background = option.Name;
        }

        private void ApplySkip(Ego ego, LifePathNode option)
        {
            ego.SkipSections.Add(option.Value);
        }

        private void ApplyMotivation(Ego ego, LifePathNode option)
        {
            ego.Motivations.Add(option.Name);
        }

        private void ApplyAge(Ego ego, LifePathNode option)
        {
            ego.EgoAge = option.Value;
        }

        private async Task ApplyForcedInterest(Ego ego, LifePathNode option)
        {
            var selectedInterest = (await ePDataService.GetCharacterGenTable("LifePathInterest")).FirstOrDefault(x => x.Name == option.Name);
            if (selectedInterest != null)
            {
                ego.CharacterGenerationNodes.Push(selectedInterest);
            }
            ego.CharacterGenerationOutput.Add($"{option.Name} {option.Description} did not load properly".Trim());
        }

        private void ApplyInterest(Ego ego, LifePathNode option)
        {
            ego.Interest = option.Name;
        }

        private void ApplyFaction(Ego ego, LifePathNode option)
        {
            ego.Faction = option.Name;
        }

        private void ApplySlight(Ego ego, LifePathNode option)
        {
            for (var i = 0; i < option.Value; i++)
            {
                ego.Psi.Slights.Add(new Slight { Name = "Random or chosen" });
            }
        }

        private async Task ApplyTrait(Ego ego, LifePathNode option)
        {
            var trait = (await ePDataService.GetTraitsAsync()).FirstOrDefault(x => x.Name == option.Name.Split("-")[0]);
            ego.EgoTraits.Add(new Trait { Name = option.Name, Description = trait?.Description ?? "", Level = option.Value, Type = "Ego" });
        }

        private async Task ApplyAptitude(Ego ego, LifePathNode option)
        {
            string aptitudeToChange = (await ePDataService.GetAptitudes()).FirstOrDefault(x => string.Equals(x.ShortName, option.Name, StringComparison.CurrentCultureIgnoreCase))?.Name ?? "";
            aptitudeToChange = char.ToUpper(aptitudeToChange[0]) + aptitudeToChange[1..];
            
            //check if aptitude already exists and add value to it
            var existingAptitude = ego.Aptitudes.FirstOrDefault(x => x.Name == aptitudeToChange);
            if (existingAptitude != null)
                existingAptitude.AptitudeValue += option.Value;
            else
                ego.Aptitudes.Add(new EgoAptitude { Name = aptitudeToChange, ShortName = option.Name, AptitudeValue = option.Value, CheckMod = 0 });
            
        }

        private void ApplyReputation(Ego ego, LifePathNode option)
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
            if (repToChange == null) return;
            repToChange.Score += option.Value;
        }

        private void ApplyPool(Ego ego, LifePathNode option)
        {
            switch (option.Name)
            {
                case "Flex":
                    ego.EgoFlex += option.Value;
                    break;
            }
        }

        private void ApplyLanguage(Ego ego, LifePathNode option)
        {
            if (string.IsNullOrEmpty(ego.Languages))
            {
                ego.Languages = option.Name;
            }
            else
            {
                ego.Languages += ", " + option.Name;
            }
        }

        private async Task ApplySkill(Ego ego, LifePathNode option)
        {
            var skills = await ePDataService.GetSkills();
            var skillName = option.Name.Split("-")[0];
            var skill = skills.FirstOrDefault(x => x.Name == option.Name.Split("-")[0]);
            SkillType skillType = skillName switch
            {
                { } s when s.Contains("know", StringComparison.OrdinalIgnoreCase) => SkillType.KnowledgeSkill,
                { } s when s.Contains("exotic", StringComparison.OrdinalIgnoreCase) => SkillType.ExoticSkill,
                _ => SkillType.EgoSkill
            };

            if (NewEgo.Skills.Any(x => x.Name == skillName))
            {
                NewEgo.Skills.First(x=>x.Name == skillName).Rank += option.Value;
                var specialization = option.Name.Split("-").Length > 1 ? option.Name.Split("-")[1] : "";
                NewEgo.Skills.First(x=>x.Name == skillName).Specialization += specialization;
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
            var selectedMorph = (await ePDataService.GetMorphs()).FirstOrDefault(x => x.Name == option.Name);
            if (selectedMorph != null)
            {
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
                    Traits = selectedMorph.MorphTraits.Select(x => new Trait { Name = x.Name, Level = x.Level }).ToList(),
                    Wares = selectedMorph.Ware.Select(x=> new Ware { Name = x}).ToList()
                });
            }
        }

        private async Task<List<LifePathNode>> ProcessTableRequest(string tableName, int tableModifier, Ego ego)
        {
            List<LifePathNode> options = new();

            var table = (await ePDataService.GetCharacterGenTable(tableName)).GetWeightedItem(tableModifier);
            
            //Console.WriteLine($"{table.Name} {table.Description}");
            ego.CharacterGenerationOutput.Add($"{table.Name} {table.Description}".Trim());

            if (table.OptionLists.Count == 0)
            {
                return [table];
            }
             
            foreach (var nodeList in table.OptionLists)
            {
                if (nodeList.Sum(x=>x.Weight) > 0)
                {
                    options.Add(nodeList.GetWeightedItem());
                }
                else
                {
                    options.AddRange(nodeList);
                }
            }

            return options;
        }
    }
}
