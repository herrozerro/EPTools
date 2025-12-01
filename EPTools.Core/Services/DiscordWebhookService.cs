using System.Text;
using System.Text.Json;
using EPTools.Core.Models;

namespace EPTools.Core.Services;

public class DiscordWebhookService(HttpClient httpClient)
{
    private HttpClient HttpClient { get; set; } = httpClient;

    public async Task SendWebhook(DiscordWebHookMessage message)
    {
        var url = "";

        var payload = new StringContent(JsonSerializer.Serialize(message),
            Encoding.UTF8, "application/json"); 
        await HttpClient.PostAsync(url, payload);
    }
}