using EPTools.Core.Models.Ego;

namespace EPTools.Blazor.ViewModels;

public class SkillViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Aptitude { get; set; } = string.Empty;
    public int Rank { get; set; }
    public int TotalValue { get; set; }
    public SkillType Type { get; set; }
}