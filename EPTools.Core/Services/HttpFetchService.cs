using System.Net.Http.Json;
using EPTools.Core.Interfaces;

namespace EPTools.Core.Services;

public class HttpFetchService(IHttpClientFactory factory) : IFetchService
{
    public async Task<T> GetTFromFileAsync<T>(string filename) where T : new()
    {
        var client = factory.CreateClient("EPClient");
        return await client.GetFromJsonAsync<T>($"data/{filename}.json") ?? new T();
    }

    public async Task<T> GetTFromEpFileAsync<T>(string filename) where T : new()
    {
        var client = factory.CreateClient("EPClient");
        return await client.GetFromJsonAsync<T>($"data/EP-Data/{filename}.json") ?? new T();
    }
}