using EPTools.Core.Interfaces;
using EPTools.Core.Models.Ego;

namespace EPTools.Core.Services;

public class EgoService(IEpDataService dataService) : IEgoService
{
    private IEpDataService DataService { get; set; } = dataService;

    public async Task<Ego> GetDefaults()
    {
        Ego newEgo = new();

        var aptitudes = await DataService.GetAptitudesAsync();
        var skills = await DataService.GetSkillsAsync();
        
        foreach (var aptitude in aptitudes)
        {
            newEgo.Aptitudes.Add(new EgoAptitude
            {
                AptitudeValue = 0,
                Name = aptitude.Name,
                ShortName = aptitude.ShortName.ToUpper()
            });
        }

        foreach (var skill in skills)
        {
            SkillType skillType = skill switch
            {
                { Know: true } => SkillType.KnowledgeSkill,
                { Name: var n } when n.StartsWith("Exotic Skill", StringComparison.OrdinalIgnoreCase) => SkillType.ExoticSkill,
                _ => SkillType.EgoSkill
            };
            newEgo.Skills.Add(new EgoSkill { Name = skill.Name, Rank = 0, Specialization = "", Aptitude = skill.Aptitude, SkillType = skillType });
        }
        
        newEgo.Identities.Add(new Identity{Alias = "Default Identity"});
        
        return newEgo;
    }
}