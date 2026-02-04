using EPTools.Core.Models.Ego;

namespace EPTools.Core.Models.LifePathGen;

public class LifepathContext(Ego.Ego ego)
{
    public Ego.Ego Ego { get; } = ego;
    public Stack<LifePathNode> Nodes { get; } = new();
    public List<int> SkipSections { get; } = [];
    public List<string> Output { get; } = [];
    public List<string> Choices { get; } = [];
}
