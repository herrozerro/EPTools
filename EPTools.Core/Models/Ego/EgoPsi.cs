namespace EPTools.Core.Models.Ego;

public class EgoPsi
{
 
    public string Substrain { get; set; } = string.Empty;
    public int InfectionRating { get; set; }
    public List<string> InfectionEvents { get; set; } = [];
    public List<EgoSleight> Sleights { get; set; } = [];
}