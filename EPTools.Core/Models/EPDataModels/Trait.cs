namespace EPTools.Core.Models.EPDataModels;

public class Trait : EpModel
{
    public string Type { get; set; } = string.Empty;
    public List<int> Cost { get; set; } = [];
    public bool Ego { get; set; }
    public bool Morph { get; set; }
    public bool Auto { get; init; }
    public bool Noted { get; init; }
}