namespace EPTools.Core.Interfaces
{
    public interface IFetchService
    {
        Task<T> GetTFromEpFileAsync<T>(string filename) where T : new();
        Task<T> GetTFromFileAsync<T>(string filename) where T : new();
    }
}