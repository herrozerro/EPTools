using EPTools.Core.Models.Ego;

namespace EPTools.Core.Services;


public class EgoService(EPDataService dataService)
{
    private EPDataService DataService { get; set; } = dataService;

    public async Task<Ego> GetDefaults()
    {
        Ego newEgo = new();

        var aptitudes = await DataService.GetAptitudes();
        var skills = await DataService.GetSkills();
        
        foreach (var aptitude in aptitudes)
        {
            newEgo.Aptitudes.Add(new EgoAptitude
            {
                AptitudeValue = 0,
                Name = aptitude.Name,
                ShortName = aptitude.ShortName
            });
        }

        foreach (var skill in skills)
        {
            SkillType skillType = skill.Name switch
            {
                { } s when s.Contains("know", StringComparison.OrdinalIgnoreCase) => SkillType.KnowledgeSkill,
                { } s when s.Contains("exotic", StringComparison.OrdinalIgnoreCase) => SkillType.ExoticSkill,
                _ => SkillType.EgoSkill
            };
            newEgo.Skills.Add(new EgoSkill { Name = skill.Name, Rank = 0, Specialization = "", Aptitude = skill.Aptitude, SkillType = skillType });
        }
        
        newEgo.Identities.Add(new Identity{Alias = "Default Identity"});
        
        return newEgo;
    }
}