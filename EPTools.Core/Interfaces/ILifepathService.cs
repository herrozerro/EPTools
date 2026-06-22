using EPTools.Core.Models.Ego;

namespace EPTools.Core.Interfaces;

public interface ILifepathService
{
    Task<Ego> GenerateEgo();
}
