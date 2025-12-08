using Blazored.LocalStorage;
using EPTools.Core.Interfaces;

namespace EPTools.Blazor.Services;

public class BlazorUserDataStore(ILocalStorageService localStorage) : IUserDataStore
{
    public async Task SaveItemAsync<T>(string key, T item) 
        => await localStorage.SetItemAsync(key, item);

    public async Task<T?> GetItemAsync<T>(string key) 
        => await localStorage.GetItemAsync<T>(key);

    public async Task DeleteItemAsync(string key) 
        => await localStorage.RemoveItemAsync(key);
}