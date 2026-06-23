using EPTools.Core.Models.Ego;

namespace EPTools.Core.Interfaces;

public interface IEgoService
{
    Task<Ego> GetDefaults();
}
