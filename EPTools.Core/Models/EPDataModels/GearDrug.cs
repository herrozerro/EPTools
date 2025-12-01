namespace EPTools.Core.Models.EPDataModels;

public class GearDrug : Gear
{
    public string Type { get; init; } = string.Empty;
    public string Application { get; init; } = string.Empty;
    public string Duration { get; init; } = string.Empty;
    public string Addiction { get; init; } = string.Empty;
}