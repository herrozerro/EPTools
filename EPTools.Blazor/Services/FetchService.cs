using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EPTools.Blazor.Services
{
    public class FetchService
    {
        private HttpClient httpClient { get; set; }

        public FetchService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> GetTFromFileAsync<T> (string filename)
        {
            return await httpClient.GetFromJsonAsync<T>($"data/{filename}.json");
        }
    }
}
