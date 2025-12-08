using System.Text.Json;
using EPTools.Core.Interfaces;

namespace EPTools.Core.Services;

public class FileUserDataStore : IUserDataStore
{
    private const string Folder = "user_data";

    public FileUserDataStore()
    {
        if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);
    }

    public async Task SaveItemAsync<T>(string key, T item)
    {
        var path = Path.Combine(Folder, $"{key}.json");
        var json = JsonSerializer.Serialize(item);
        await File.WriteAllTextAsync(path, json);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var path = Path.Combine(Folder, $"{key}.json");
        if (!File.Exists(path)) return default;

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task DeleteItemAsync(string key)
    {
        var path = Path.Combine(Folder, $"{key}.json");
        if (File.Exists(path)) File.Delete(path);
    }
}