namespace EPTools.Core.Models.Ego;

public class EgoSkill
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int Rank { get; set; }
    public string Aptitude { get; set; } = string.Empty;
    public int Modifier { get; set; }
    public int SkillTotal { get; set; }
        
    public SkillType SkillType { get; init; }
}

public enum SkillType
{
    EgoSkill,
    ExoticSkill,
    KnowledgeSkill
}