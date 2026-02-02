namespace EPTools.Core.Models.Ego;

public sealed class EgoAptitude
{
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public int AptitudeValue { get; set; }
    public int CheckMod { get; set; }

    public int CheckRating => AptitudeValue * 3 + CheckMod;
}