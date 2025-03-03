using System.Text.Json;
using EPTools.Core.Interfaces;

namespace EPTools.Core.Services
{
    public class FileFetchService : IFetchService
    {
        public async Task<T> GetTFromFileAsync<T>(string filename) where T : new()
        {
            filename = filename.ToLower();

            if (!File.Exists($"data/{filename}.json"))
            {
                return new T();
            }

            await using var fileStream = new FileStream($"data/{filename}.json", FileMode.Open);
            
            var item = await JsonSerializer.DeserializeAsync<T>(fileStream);

            if (item == null)
            {
                throw new NullReferenceException();
            }

            return item;
        }

        public async Task<T> GetTFromEpFileAsync<T>(string filename) where T : new()
        {
            //filename = filename.ToLower();
            Console.WriteLine(filename);
            if (!File.Exists($"./data/EP-Data/{filename}.json"))
            {
                return new T();
            }

            await using var textStream = new FileStream($"data/EP-Data/{filename}.json", FileMode.Open);
            
            var item = await JsonSerializer.DeserializeAsync<T>(textStream);

            if (item == null)
            {
                throw new NullReferenceException();
            }

            return item;
        }
    }
}
