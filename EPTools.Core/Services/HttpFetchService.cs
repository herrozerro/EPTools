using System.Net.Http.Json;
using EPTools.Core.Interfaces;

namespace EPTools.Core.Services
{
    public class HttpFetchService(HttpClient httpClient) : IFetchService
    {
        private HttpClient HttpClient { get; set; } = httpClient;

        public async Task<T> GetTFromFileAsync<T>(string filename) where T : new()
        {
            //filename = filename.ToLower();

            return await HttpClient.GetFromJsonAsync<T>($"data/{filename}.json") ?? new T();
        }

        public async Task<T> GetTFromEpFileAsync<T>(string filename) where T : new()
        {
            //filename = filename.ToLower();
            return await HttpClient.GetFromJsonAsync<T>($"data/EP-Data/{filename}.json") ?? new T();
        }
    }
}
