namespace EPTools.Core.Interfaces;

public interface IUserDataStore
{
    Task SaveItemAsync<T>(string key, T item);
    Task<T?> GetItemAsync<T>(string key);
    Task DeleteItemAsync(string key);
}